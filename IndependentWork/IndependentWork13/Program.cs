using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
class Program
{
    public static List<int> GenerateData(int count)
    {
        List<int> data = new List<int>();
        Random random = new Random();

        for (int i = 0; i < count; i++)
        {
            data.Add(random.Next(1, 10000000));
        }
        return data;
    }

    public static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;

    }

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nВведіть кількість елементів (1000000, 5000000 або 10000000):");

                if (!int.TryParse(Console.ReadLine(), out int userInput) ||
                    (userInput != 1000000 && userInput != 5000000 && userInput != 10000000))
                {
                    Console.WriteLine("Некоректне значення. Спробуйте ще раз.");
                    continue;
                }
            

            List<int> data = GenerateData(userInput);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var linqResults = data.Where(number => IsPrime(number)).Select(number => number * 2).ToList();
            stopwatch.Stop();
            Console.WriteLine($"Час виконання за допомогою LINQ: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");

            stopwatch.Reset();
            stopwatch.Start();
            var plinqResults = data.AsParallel().Where(nums => IsPrime(nums)).Select(nums => nums * 2).ToList();
            stopwatch.Stop();
            Console.WriteLine($"Час виконання за допомогою PLINQ: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");

            stopwatch.Reset();
            stopwatch.Start();
            int counter = 0;
            data.AsParallel().ForAll(nums => counter ++);
            stopwatch.Stop();
            Console.WriteLine($"Час виконання з проблемою з PLINQ: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");
            Console.WriteLine("Лічильник: " + counter);
            
            int safeCounter = 0;
            data.AsParallel().ForAll(nums => Interlocked.Increment(ref safeCounter));
            Console.WriteLine("Потокобезпечний лічильник: " + safeCounter);

            Console.WriteLine("Enter - продовжити || exit - вийти.");

            if (Console.ReadLine()?.ToLower() == "exit")
            {
                break;
            }
        }


    }
}
