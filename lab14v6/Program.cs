using System;
using System.Collections.Generic;

class Inventory
{
    private HashSet<string> category = new HashSet<string>();
    public void AddCategory(string name)
    {
        category.Add(name);
    }

    public bool HasCategory(string name)
    {
        return category.Contains(name);
    }

    public bool RemoveCategory(string name)
    {
        return category.Remove(name);
    }


    private Queue<string> orders = new Queue<string>();
    public void AddOrder(string order)
    {
        orders.Enqueue(order);
    }

    public string PeekOrder()
    {
        return orders.Peek();
    }

    public string DequeueOrder()
    {
        return orders.Dequeue();
    }


    private Stack<string> history = new Stack<string>();
    public void AddHistory(string operation)
    {
        history.Push(operation);
    }

    public string PopHistory()
    {
        return history.Pop();
    }

    public string PeekHistory()
    {
        return history.Peek();
    }
}

public class Program
{
    static void Main()
    {
        Inventory inventory = new Inventory();

        inventory.AddCategory("Техніка");
        inventory.AddCategory("Одяг");
        inventory.AddCategory("Техніка");
        Console.WriteLine("Категорія: Техніка існує: " + inventory.HasCategory("Техніка"));
        inventory.RemoveCategory("Одяг");
        Console.WriteLine("Категорію 'Одяг' видалено.");


        inventory.AddOrder("Ноутбук");
        inventory.AddOrder("Мишка");
        Console.WriteLine("Наступне замовлення: " + inventory.PeekOrder());
        Console.WriteLine("Оброблено замовлення: " + inventory.DequeueOrder());
        Console.WriteLine("Наступне замовлення: " + inventory.PeekOrder());

        inventory.AddHistory("Додавання товару");
        inventory.AddHistory("Видалення товару");
        Console.WriteLine("Остання операція: " + inventory.PeekHistory());
        Console.WriteLine("Вилучено з історії: " + inventory.PopHistory());
        Console.WriteLine("Попередня операція: " + inventory.PeekHistory());


    }
}