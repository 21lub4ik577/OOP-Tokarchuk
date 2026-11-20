
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

public class Program
{
    public static async Task Main()
    {
        await RunDatabaseScenario();
        await RunQueueScenario();
        await RunLongOperationScenario();

        Console.WriteLine("\nУсі сценарії виконано.");
    }
    
    private static async Task RunDatabaseScenario()
    {
        Console.WriteLine("=== 1. База даних: Retry + Timeout ===");

        int attempt = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<InvalidOperationException>()
                    .Handle<TimeoutRejectedException>(),

                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(150),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,

                OnRetry = static args =>
                {
                    Console.WriteLine(
                        $"[БД] Повтор #{args.AttemptNumber + 1}: " +
                        $"{args.Outcome.Exception?.GetType().Name}; " +
                        $"затримка {args.RetryDelay.TotalMilliseconds:F0} мс");

                    return default;
                }
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMilliseconds(400),

                OnTimeout = static args =>
                {
                    Console.WriteLine(
                        $"[БД] Перевищено час очікування: {args.Timeout}");

                    return default;
                }
            })
            .Build();

        try
        {
            string result = await pipeline.ExecuteAsync(
                async cancellationToken =>
                {
                    attempt++;

                    Console.WriteLine(
                        $"[БД] Спроба {attempt}: читання даних...");

                    if (attempt == 1)
                    {
                        throw new InvalidOperationException(
                            "БД тимчасово недоступна");
                    }

                    await Task.Delay(
                        attempt == 2 ? 550 : 100,
                        cancellationToken);

                    return "замовлення користувача";
                });

            Console.WriteLine($"[БД] Результат: {result}\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                $"[БД] Помилка після усіх спроб: {exception.Message}\n");
        }
    }


    private static async Task RunQueueScenario()
    {
        Console.WriteLine(
            "=== 2. Черга повідомлень: Circuit Breaker + Fallback ===");

        int attempt = 0;

        var pipeline = new ResiliencePipelineBuilder<string>()
            .AddFallback(new FallbackStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>()
                    .Handle<IOException>()
                    .Handle<BrokenCircuitException>(),

                FallbackAction = static args =>
                {
                    Console.WriteLine(
                        "[Черга] Fallback: повідомлення збережено локально");

                    return Outcome.FromResultAsValueTask(
                        "повідомлення очікує повторної відправки");
                },

                OnFallback = static args =>
                {
                    Console.WriteLine(
                        "[Черга] Активовано резервний режим");

                    return default;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>()
                    .Handle<IOException>(),

                FailureRatio = 1.0,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(1),

                OnOpened = static args =>
                {
                    Console.WriteLine(
                        "[Черга] Circuit Breaker OPEN");

                    return default;
                },

                OnHalfOpened = static args =>
                {
                    Console.WriteLine(
                        "[Черга] Circuit Breaker HALF-OPEN");

                    return default;
                },

                OnClosed = static args =>
                {
                    Console.WriteLine(
                        "[Черга] Circuit Breaker CLOSED");

                    return default;
                }
            })
            .Build();

        for (int i = 1; i <= 4; i++)
        {
            attempt++;

            string result = await pipeline.ExecuteAsync(
                async cancellationToken =>
                {
                    Console.WriteLine(
                        $"[Черга] Спроба відправки #{attempt}");

                    await Task.Delay(20, cancellationToken);

                    if (attempt <= 2)
                    {
                        throw new IOException(
                            "Черга перевантажена");
                    }

                    return "повідомлення відправлено";
                });

            Console.WriteLine(
                $"[Черга] Результат #{i}: {result}");
        }

        // Очікуємо завершення періоду OPEN.
        await Task.Delay(TimeSpan.FromSeconds(1.1));

        // Перевірочний виклик у стані HALF-OPEN.
        attempt++;

        string recoveryResult = await pipeline.ExecuteAsync(
            async cancellationToken =>
            {
                Console.WriteLine(
                    $"[Черга] Перевірочна спроба #{attempt}");

                await Task.Delay(20, cancellationToken);

                return "повідомлення успішно відправлено";
            });

        Console.WriteLine(
            $"[Черга] Результат відновлення: {recoveryResult}\n");
    }


    private static async Task RunLongOperationScenario()
    {
        Console.WriteLine(
            "=== 3. Генерація звіту: Timeout + Fallback ===");

        var pipeline = new ResiliencePipelineBuilder<string>()
            .AddFallback(new FallbackStrategyOptions<string>
            {
                ShouldHandle = new PredicateBuilder<string>()
                    .Handle<TimeoutRejectedException>(),

                FallbackAction = static args =>
                {
                    Console.WriteLine(
                        "[Звіт] Fallback: повернено кешовану версію");

                    return Outcome.FromResultAsValueTask(
                        "кешований звіт за попередній період");
                },

                OnFallback = static args =>
                {
                    Console.WriteLine(
                        "[Звіт] Основна генерація не встигла завершитися");

                    return default;
                }
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMilliseconds(300),

                OnTimeout = static args =>
                {
                    Console.WriteLine(
                        $"[Звіт] Таймаут: {args.Timeout.TotalMilliseconds:F0} мс");

                    return default;
                }
            })
            .Build();

        string result = await pipeline.ExecuteAsync(
            async cancellationToken =>
            {
                Console.WriteLine(
                    "[Звіт] Початок генерації...");

                await Task.Delay(
                    TimeSpan.FromMilliseconds(700),
                    cancellationToken);

                return "свіжий звіт";
            });

        Console.WriteLine(
            $"[Звіт] Повернуте значення: {result}\n");
    }
}