using System;
class IntStack
{
    private List<int> _items = new List<int>();
    public int this[int index]
    {
        get { return _items[index]; }
        set { _items[index] = value; }
    }
    
    public void Push(int item)
    {
        _items.Add(item);
    }

    public int Pop()
    {
        int item = _items[_items.Count - 1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    public int Peek()
    {
        return _items[_items.Count - 1];
    }

    public int Count
    {
        get { return _items.Count; }
    }

    public static IntStack operator +(IntStack a, IntStack b)
    {
        IntStack result = new IntStack();
        result._items.AddRange(a._items);
        result._items.AddRange(b._items);
        return result;
    }

    public static bool operator ==(IntStack a, IntStack b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(IntStack a, IntStack b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not IntStack other)
            return false;
        if (_items.Count != other._items.Count)
            return false;
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != other._items[i])
                return false;
        }
        return true;
    }
    
    public override int GetHashCode()
    {
        int hash = 17;
        foreach (int item in _items)
        {
            hash = hash * 31 + item;
        }
        return hash;
    }

    public override string ToString()
    {
        return "[" + string.Join(", ", _items) + "]";
    }
}

class Program
{
    static void Main()
    {
        IntStack stack1 = new IntStack();
        IntStack stack2 = new IntStack();

        stack1.Push(10);
        stack1.Push(20);
        stack1.Push(30);

        stack2.Push(10);
        stack2.Push(20);
        stack2.Push(30);

        Console.WriteLine(stack1[0]);
        stack1[0] = 100;
        Console.WriteLine(stack1[0]);

        Console.WriteLine(stack1.Peek());
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.Count);

        IntStack stack3 = stack1 + stack2;

        Console.WriteLine(stack3);
        Console.WriteLine(stack1 == stack2);
        Console.WriteLine(stack1 != stack2);
    }

    
}