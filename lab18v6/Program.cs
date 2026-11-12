using System;
using System.Linq;
public class ImageProcessingException : Exception
{
    public string ImageName { get; set;}
    public string Operation { get;set; }

    public ImageProcessingException(string imageName, string operation) : base($"Помилка під час викнання операції '{operation}' для зображення '{imageName}'.")
    {
        ImageName = imageName;
        Operation = operation;
    }

    public ImageProcessingException(string message) : base(message)
    {
        ImageName = "Невідоме зображення";
        Operation = "Невідома операція";
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
}

public sealed class Result<T>
{
    public bool IsSuccess { get; set;}
    public T? Value { get; set;}
    public string Error { get; set;}

    private Result(bool isSuccess, T? value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? string.Empty;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static Result<T> Failure(string error) => new(false, default, error);
    public override string ToString() => IsSuccess ? $"Success: {Value}" : $"Failure: {Error}";
}

public class ImageProcessor
{
    private static readonly string[] SupportedFilters =
    { "grayscale", "sepia", "contrast", "blur"};

    public byte[] ResizeImage(byte[] imageData, int width, int height)
    {
        if (imageData == null || imageData.Length == 0)
        {
            throw new ImageProcessingException("Невідоме зображення", "ResizeImage");
        }

        if (width <= 0 || height <= 0)
        {
            throw new ImageProcessingException("Невідоме зображення", "ResizeImage");
        }

        if (!IsValidImageData(imageData))
        {
            throw new ImageProcessingException("Невідоме зображення", "ResizeImage");
        }

        int outputSize = Math.Max(1, width * height * 3);
        byte[] resizedImage = new byte[outputSize];

        for (int i = 0; i < resizedImage.Length; i++)
        {
            resizedImage[i] = imageData[i % imageData.Length];
        }

        return resizedImage;
    }

    public Result<bool> ApplyFilter(byte[] imageData, string filterName)
    {
        if (imageData == null || imageData.Length == 0 || !IsValidImageData(imageData))
        {
            return Result.Failure<bool>("Зображення пошкоджене або порожнє.");
        }

        if (string.IsNullOrWhiteSpace(filterName))
        {
            return Result.Failure<bool>("Назва фільтра не може бути порожньою.");
        }

        string normalizedFilter = filterName.Trim();
        if (!SupportedFilters.Contains(normalizedFilter, StringComparer.OrdinalIgnoreCase))
        {
            return Result.Failure<bool>($"Фільтр '{filterName}' невідомий.");
        }

        return Result.Success(true);
    }

    private static bool IsValidImageData(byte[] imageData)
    {
        if (imageData == null || imageData.Length < 8)
        {
            return false;
        }

        byte[][] signatures =
        {
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
            new byte[] { 0xFF, 0xD8, 0xFF },
            new byte[] { 0x42, 0x4D },
            new byte[] { 0x47, 0x49, 0x46, 0x38 }
        };

        foreach (byte[] signature in signatures)
        {
            if (imageData.Length >= signature.Length &&
                imageData.Take(signature.Length).SequenceEqual(signature))
            {
                return true;
            }
        }

        return false;
    }
}

public class Program
{
    public static void Main()
    {
        ImageProcessor processor = new ImageProcessor();

        byte[] validImage = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x00 };
        byte[] corruptedImage = new byte[] { 0x12, 0x34, 0x56, 0x78 };

        Console.WriteLine("_ResizeImage_");
        try
        {
            byte[] resized = processor.ResizeImage(validImage, 160, 120);
            Console.WriteLine($"Розмір зображення після зміни: {resized.Length} байт.");
        }
        catch (ImageProcessingException ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            processor.ResizeImage(corruptedImage, 100, 100);
        }
        catch (ImageProcessingException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();
        Console.WriteLine("_ApplyFilter_");

        Result<bool> grayscaleResult = processor.ApplyFilter(validImage, "grayscale");
        Console.WriteLine(grayscaleResult.IsSuccess ? "Фільтр 'grayscale' застосовано успішно." : grayscaleResult.Error);

        Result<bool> unknownFilterResult = processor.ApplyFilter(validImage, "noise");
        Console.WriteLine(unknownFilterResult.IsSuccess ? "Фільтр застосовано успішно." : unknownFilterResult.Error);

        Result<bool> corruptedResult = processor.ApplyFilter(corruptedImage, "sepia");
        Console.WriteLine(corruptedResult.IsSuccess ? "Фільтр застосовано успішно." : corruptedResult.Error);
    }
}

