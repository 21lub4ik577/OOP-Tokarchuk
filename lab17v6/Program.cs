using System;
using System.IO;
using System.Net.Http;
using System.Threading;

public class FileProcessor
{
    private int _attempts;
    public string GetLogContent(string path)
    {
        _attempts++;
        Console.WriteLine($"[FileProcessor] Attempting to read file: {path}");
        if (_attempts <= 2)
        {
            throw new FileNotFoundException($"Temporary error: log file '{path}' is not available yet.", path);
        }
        Console.WriteLine("[FileProcessor] Log file read successfully.");
        return "INFO: Application started.";
    }
}

public class NetworkClient
{
    private int _attempts;
    public void SendLogToServer(string url, string logContent)
    {
        _attempts++;
        Console.WriteLine($"[NetworkClient] Sending log to {url}: {logContent}");

        if (_attempts <= 4)
        {
            throw new HttpRequestException( $"Temporary network error on attempt {_attempts}.");
        }
        Console.WriteLine("[NetworkClient] Log sent successfully.");
    }
}

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        if (retryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(retryCount));
        }

        if (initialDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(initialDelay));
        }

        shouldRetry ??= _ => true;

        for (int attempt = 1; ; attempt++)
        {
            try
            {
                Console.WriteLine($"[RetryHelper] Attempt {attempt}/{retryCount + 1}.");
                T result = operation();
                Console.WriteLine($"[RetryHelper] Operation succeeded on attempt {attempt}.");
                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"[RetryHelper] Error: {exception.GetType().Name}: {exception.Message}");

                if (attempt > retryCount || !shouldRetry(exception))
                {
                    Console.WriteLine("[RetryHelper] No retry will be performed.");
                    throw;
                }

                TimeSpan delay = TimeSpan.FromMilliseconds(
                    initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

                if (delay > TimeSpan.Zero)
                {
                    Console.WriteLine($"[RetryHelper] Waiting {delay.TotalMilliseconds} ms.");
                    Thread.Sleep(delay);
                }
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine();

        Func<Exception, bool> retryFileErrors = exception => exception is FileNotFoundException;
        Func<Exception, bool> retryNetworkErrors = exception => exception is HttpRequestException;
        Func<Exception, bool> retryVariantErrors = exception => exception is FileNotFoundException or HttpRequestException;

        var fileProcessor = new FileProcessor();
        string logContent = RetryHelper.ExecuteWithRetry(
            () => fileProcessor.GetLogContent("logs/application.log"), retryCount: 3, initialDelay: TimeSpan.FromMilliseconds(100), shouldRetry: retryFileErrors);
        Console.WriteLine($"[Main] Retrieved log content: {logContent}");
        Console.WriteLine();

        var networkClient = new NetworkClient();
        RetryHelper.ExecuteWithRetry(
            () =>
            {
                networkClient.SendLogToServer( "https://api.example.com/logs", logContent);
                return true;
            },
            retryCount: 5,
            initialDelay: TimeSpan.FromMilliseconds(100),
            shouldRetry: retryNetworkErrors);
        Console.WriteLine("[Main] Log delivery completed.");
        Console.WriteLine();

        Console.WriteLine($"[Main] shouldRetry for FileNotFoundException: {retryVariantErrors(new FileNotFoundException())}");
        Console.WriteLine( $"[Main] shouldRetry for HttpRequestException: {retryVariantErrors(new HttpRequestException())}");
        Console.WriteLine($"[Main] shouldRetry for InvalidOperationException: {retryVariantErrors(new InvalidOperationException())}");
        Console.WriteLine();
    }
}