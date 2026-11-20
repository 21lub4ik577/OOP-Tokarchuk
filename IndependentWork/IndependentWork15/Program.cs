using System;
using System.Threading;

public class RetryPolicy
{
    private int _maxRetries;
    private TimeSpan _initialDelay;
    private double _jitterFactor;
    private Random _random = new Random();


    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        for (int i = 0; i <= _maxRetries; i++)
        {
            Console.WriteLine($"номер поточної спроби:  {i + 1} загальна кількість спроб: {_maxRetries + 1}");
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Причина невдачі: {ex.Message}");
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    throw;
                }
                if (i < _maxRetries)
                {
                    double delay = _initialDelay.TotalMilliseconds * Math.Pow(2, i) * (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);
                    Console.WriteLine($"Затримка перед наступною спробою: {delay:F0} мс");
                    Thread.Sleep((int)delay);
                }
            }
        }
        throw new Exception("Усі спроби виконання операції завершилися невдало.");
    }
}

class Program
{
    static void Main()
    {
        RetryPolicy retryPolicy = new RetryPolicy( maxRetries: 3, initialDelay: TimeSpan.FromMilliseconds(500), jitterFactor: 0.2
        );

        Console.WriteLine("_СЦЕНАРІЙ 1: УСПІШНЕ ВИКОНАННЯ_");
        int attempts = 0;
        try
        {
            string result = retryPolicy.Execute(() =>
            {
                attempts++;

                if (attempts < 3)
                {
                    throw new Exception("Тимчасова помилка операції.");
                }

                return "Операцію успішно виконано!";
            }, ex => ex.Message.Contains("Тимчасова"));
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }

        Console.WriteLine("\n_СЦЕНАРІЙ 2: ПОСТІЙНА НЕВДАЧА_");
        try
        {
            string result = retryPolicy.Execute<string>(() =>
            {
                throw new Exception("Сервер недоступний.");
            });

            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }
    }
}