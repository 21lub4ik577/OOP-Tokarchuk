using System;

class Bicycle
{
    private string _bicycle_name;
    private string _bicycle_color;
    private int _bicycle_price;

    public string BicycleName
    {
        get{return _bicycle_name;}
        private set{_bicycle_name = value;}
    }

    public Bicycle(string bicycle_color, string bicycle_name, int bicycle_price)
    {
        _bicycle_color = bicycle_color;
        _bicycle_name = bicycle_name;
        _bicycle_price = bicycle_price;
    }

    public void BicycleShowInfo()
    {
        if(_bicycle_price <= 30000)
        {
            Console.WriteLine(_bicycle_color + " " + _bicycle_name + " bicycle cost - " + _bicycle_price + ", is not expensive ");
        }
        else
        {
            Console.WriteLine(_bicycle_color + " " + _bicycle_name + " bicycle cost - " + _bicycle_price + ", is expensive ");
        }
    }
}

class Book
{
    private string _book_author;
    private string _book_name;

    public string BookName
    {
        get{ return _book_name;}
        private set { _book_name = value;}
    }

    public Book(string book_name, string book_author)
    {
        _book_name = book_name;
        _book_author = book_author;
    }

    public void BookShowInfo()
    {
        Console.WriteLine("Book: " + _book_name + " by author: " + _book_author);
    }
}

class Animal
{
    private string _animal_name;
    private string _animal_sound;

    public string AnimalName
    {
        get{ return _animal_name;}
        set{ _animal_name = value;}
    }

    public Animal(string animal_name, string animal_sound)
    {
        _animal_name = animal_name;
        _animal_sound = animal_sound;
    }

    public void AnimalShowInfo()
    {
        Console.WriteLine(_animal_name + " makes a sound: " + _animal_sound);
    }
}

public class Program
{
    public static void Main()
    {
        Bicycle bicycle1 = new Bicycle("Green", "Trek", 50000);
        bicycle1.BicycleShowInfo();

        Book book1 = new Book("It", "Stephen King");
        book1.BookShowInfo();

        Animal animal1 = new Animal("Dog", "Gav-Gav-Gav");
        animal1.AnimalShowInfo();

    }
}