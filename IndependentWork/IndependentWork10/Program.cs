using System;
using System.Collections.Generic;
public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(IEnumerable<TSource> collection, Func<TSource, TResult> mapper)
    {
        var result = new List<TResult>();
        foreach (var item in collection)
        {
            result.Add(mapper(item));
        }
        return result;
    }
    public static T MaxBy<T, TKey>(IEnumerable<T> collection, Func<T, TKey> keySelector)
    where TKey : IComparable<TKey>
    {
        T? maxItem = default(T);
        TKey? maxKey = default(TKey);
        bool first = true;
        foreach (var item in collection)
        { 
            TKey key = keySelector(item);
            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
        {
            throw new InvalidOperationException();
        }

        return maxItem!;
    }

    public static IEnumerable<T> Filter<T>(IEnumerable<T> collection, Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static void ForEach<T>(IEnumerable<T> collection,Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}

class Program
{
    public class Product
    {
        public string Name { get; set;} = " ";
        public decimal Price { get; set; }
    }
    static void Main()
    {
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 25000 },
            new Product { Name = "Mouse", Price = 500 },
            new Product { Name = "Keyboard", Price = 1500 }
        };
        var ints = new List<int> {10, 20, 0, 3, 33};
        var strings = new List<string> {"Car", "Banana", "Creep", "Tree"};


        Console.WriteLine("\nMap:");
        var names = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("Names: " + string.Join(", ", names));

        var doubledNumbers = CollectionUtils.Map(ints, n => n * 2);
        Console.WriteLine("Doubled: " + string.Join(", ", doubledNumbers));

        var stringLengths = CollectionUtils.Map(strings, s => s.Length);
        Console.WriteLine("Lengths: " + string.Join(", ", stringLengths));


        Console.WriteLine("\nMaxBy:");
        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Most expensive: {mostExpensive.Name} - {mostExpensive.Price}");

        var maxNum = CollectionUtils.MaxBy(ints, m => m);
        Console.WriteLine("Max number: " + maxNum);

        var maxLength = CollectionUtils.MaxBy(strings, ms => ms.Length);
        Console.WriteLine("Max length: " + maxLength);


        Console.WriteLine("\nFilter:");
        var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
        Console.WriteLine("Expensive products:");
        CollectionUtils.ForEach(expensive, p => Console.WriteLine($" - {p.Name}"));

        var filterNums = CollectionUtils.Filter(ints, fn => fn > 10);
        Console.WriteLine("Numbers > 10: " + string.Join(", ", filterNums));

        var filterStrings = CollectionUtils.Filter(strings, fs => fs.Length > 4);
        Console.WriteLine("Words longer than 4: " + string.Join(", ", filterStrings));


        Console.WriteLine("\nForEach:");
        CollectionUtils.ForEach(products, p => Console.WriteLine($"{p.Name}: {p.Price} грн"));
        
        CollectionUtils.ForEach(ints, ife => Console.WriteLine($"Number: {ife}"));

        CollectionUtils.ForEach(strings, sfe => Console.WriteLine($"Word: {sfe}"));
    }
}