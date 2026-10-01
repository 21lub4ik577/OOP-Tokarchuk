using System;

public class Dish
{
    private string _name;

    public Dish(string name)
    {
        _name = name;
    }

    public virtual void Prepare()
    {
        
    }

    public string Name
    {
        get{ return _name;}
    }
}

public class Pizza : Dish
{
    private string _toppings;

    public Pizza(string name, string toppings) : base(name)
    {
        _toppings = toppings;
    }

    public override void Prepare()
    {
        Console.WriteLine("Preparing pizza "+ Name + " with toppings: " + _toppings + ".");
    }
}

public class Soup : Dish
{
    private string _brothType;

    public Soup(string name, string brothType) : base(name)
    {
        _brothType = brothType;
    }

    public override void Prepare()
    {
        Console.WriteLine("Preparing soup " + Name + " with broth: " + _brothType + ".");
    }
}

public class Salad : Dish
{
    private string _dressing;

    public Salad(string name, string dressing) : base(name)
    {
        _dressing = dressing;
    }

    public override void Prepare()
    {
        Console.WriteLine("Preparing salad " + Name + " with dressing: " + _dressing + ".");
    }
}

public class Program
{
    static void Main()
    {
        List<Dish> dishes = new List<Dish>();
        List<string> preparedDishes = new List<string>();

        dishes.Add( new Pizza("Peperoni", "cheese, pepperoni"));
        dishes.Add( new Soup("Borscht", "beef broth"));
        dishes.Add( new Salad("Caesar", "Caesar dressing"));

        foreach (Dish dish in dishes)
        {
            dish.Prepare();
            preparedDishes.Add(dish.Name);
        }
        Console.WriteLine("\nPrepared dishes:");
        foreach ( string dishName in preparedDishes)
        {
            Console.WriteLine( dishName );
        }
    }
}
