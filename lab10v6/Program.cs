using System;
using System.Collections.Generic;

public interface IConverter<TInput, TOutput>
{
    TOutput Convert(TInput input);
}

class StringToIntConverter : IConverter<string, int>
{
    public int Convert(string input)
    {
        return int.Parse(input);
    }
}

class StringToBoolConverter : IConverter<string, bool>
{
    public bool Convert(string input)
    {
        return bool.Parse(input);
    }
}

public abstract class DataProcessor
{
    public abstract void LoadData();

    public abstract void TransformData();

    public void SaveProcessedData()
    {
        Console.WriteLine("Save Processed Data");
    }
}

public class CsvProcessor : DataProcessor
{
    public override void LoadData()
    {
        Console.WriteLine("Data is loaded");
    }

    public override void TransformData()
    {
        Console.WriteLine("Data is transformed");
    }
}

public class JsonProcessor: DataProcessor
{

    public override void LoadData()
    {
        Console.WriteLine("Data is loaded");
    }
    public override void TransformData()
    {
        Console.WriteLine("Data is transformed");
    }
}

class Program
{
    static void Main()
    {
        List<IConverter<string, int>> intConverters = new()
        {
            new StringToIntConverter()
        };

        List<IConverter<string, bool>> boolConverters = new ()
        {
            new StringToBoolConverter()
        };

        foreach (IConverter<string, int> converter in intConverters)
        {
            Console.WriteLine(converter.Convert("10"));
        }

        foreach (IConverter<string, bool> converter in boolConverters)
        {
            Console.WriteLine(converter.Convert("False"));
        }

        List<DataProcessor> processors = new()
        {
            new CsvProcessor(),
            new JsonProcessor()
        };

        foreach (DataProcessor processor in processors)
        {
            processor.LoadData();
            processor.TransformData();
            processor.SaveProcessedData();
        }
    }
}