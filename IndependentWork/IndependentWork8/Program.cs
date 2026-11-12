using System;
using System.Collections.Generic;

public class Cache<TKey, TValue>
{
    private int _capacity;
    private Dictionary<TKey, TValue> _cache;
    private Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Ємність кешу повинна бути більшою за 0." , nameof(capacity));
        }
        
        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue> (capacity);
        _keysOrder = new Queue<TKey> ();
    }

    public void Add(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count >= _capacity)
        {
            while (_keysOrder.Count > 0)
            {
                TKey oldestKey = _keysOrder.Dequeue();

                if ()
                {
                    
                }
            }
        }
    }


}