using System;
using System.Collections.Generic;

public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Ємність кешу повинна бути більшою за 0.");
        }

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>();
    }

    public int Count => _cache.Count;

    public void Add(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count == _capacity)
        {
            TKey oldestKey = _keysOrder.Dequeue();
            _cache.Remove(oldestKey);
        }

        _cache.Add(key, value);
        _keysOrder.Enqueue(key);
    }

    public TValue Get(TKey key)
    {
        if (_cache.TryGetValue(key, out TValue? value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Ключ '{key}' відсутній у кеші.");
    }

    public bool ContainsKey(TKey key) => _cache.ContainsKey(key);
}

public static class Program
{
    public static void Main()
    {
        var cache = new Cache<string, string>(2);
        cache.Add("перший", "значення 1");
        cache.Add("другий", "значення 2");
        Console.WriteLine(cache.Get("перший"));

        cache.Add("третій", "значення 3");
        Console.WriteLine($"Перший ключ у кеші: {cache.ContainsKey("перший")}");
        Console.WriteLine($"Третій ключ у кеші: {cache.ContainsKey("третій")}");
        Console.WriteLine($"Кількість елементів: {cache.Count}");
    }
}
