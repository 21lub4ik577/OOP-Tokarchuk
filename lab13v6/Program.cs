using System;
using System.Collections.Generic;
public class FunctionalUtils
{
    public static List<TResult> Map<T, TResult>(List<T> list, Func<T, TResult> mapper)
    {
        var result = new List<TResult>();
        foreach (var item in list)
        {
            result.Add(mapper(item));
        }
        return result;
    }
    public static List<T> Filter<T>(List<T> list, Func<T, bool> predicate)
    {
        var result = new List<T>();
        foreach (var item in list)
        {
            if (predicate(item))
            result.Add(item);
        }
        return result;
    }

    public static T MaxBy<T, TKey>(List<T> list, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("List cannot be empty.", nameof(list));
        }
        T maxItem = list[0];
        TKey maxKey = keySelector(maxItem);
        foreach (var item in list)
        {
            TKey key = keySelector(item);
            if (key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
            }
        }
        return maxItem;
        }

    public static int Count<T>(List<T> list, Predicate<T> predicate)
    {
        int count = 0;
        foreach (var item in list)
        {
            if (predicate(item))
            {
                count++;
            }
        }

        return count;
    }
    
    public static List<T> Distinct<T>(List<T> list)
    {
        var result = new List<T>();
        foreach (var item in list)
        {
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static void ForEach<T>(List<T> list, Action<T> action)
    {
        foreach (var item in list)
        {
            action(item);
        }
    }
}

class Animal
{
    public string Name { get; set; }
    public string Species {get; set; }
    public int Age { get; set;}

    public Animal(string name, string species, int age)
    {
        Name = name;
        Species = species;
        Age = age;
    }
}

class Program
{
    static void Main()
    {
        List<Animal> animals = new List<Animal>
        {
            new Animal("Аміго", "Кіт", 3),
            new Animal("Барабан", "Собака", 5),
            new Animal("Халід-Кашмірі", "Макака", 2),
            new Animal("Коля", "Кіт", 4)
        };

        List<string> names = FunctionalUtils.Map(animals, animal => animal.Name);
        Console.WriteLine("Імена тварин:");
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }

        
        List<Animal> olderAnimals = FunctionalUtils.Filter(animals, animal => animal.Age > 3);
        Console.WriteLine("Тварини старші за 3 роки:");
        foreach (var animal in olderAnimals)
        {
            Console.WriteLine($"{animal.Name} — {animal.Age} років");
        }


        
        Animal oldestAnimal = FunctionalUtils.MaxBy(animals, animal => animal.Age);
        Console.WriteLine($"Найстаріша тварина: {oldestAnimal.Name}, вік: {oldestAnimal.Age}");


        int catCount = FunctionalUtils.Count(
            animals, animal => animal.Species == "Кіт");
        Console.WriteLine($"Кількість котів: {catCount}");

        
        List<int> numbers = new List<int> { 1, 2, 2, 3, 1, 4 };
        List<int> uniqueNumbers = FunctionalUtils.Distinct(numbers);
        Console.WriteLine("Унікальні числа:");
        foreach (var number in uniqueNumbers)
        {
            Console.WriteLine(number);
        }


        List<string> words = new List<string> { "кіт", "собака", "кіт", "папуга" };
        List<string> uniqueWords = FunctionalUtils.Distinct(words);
        Console.WriteLine("Унікальні слова:");
        foreach (var word in uniqueWords)
        {
            Console.WriteLine(word);
        }


        Console.WriteLine("Усі імена тварин:");
        FunctionalUtils.ForEach(animals, animal => Console.WriteLine(animal.Name));
    }
}