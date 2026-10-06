using System;

abstract class StorageService
{
    
    public abstract void Save(string filename, byte[] data);
}

class LocalStorage : StorageService
{
    private string _path;
    public LocalStorage(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be empty.");
        }

        _path = path;
    }
    public override void Save(string filename, byte[] data)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be empty.");
        }

        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Data cannot be empty.");
        }

        Console.WriteLine("Файл: " + filename + " збережено в: " + _path);
    }
}
class CloudStorage : StorageService
{
    private string _cloudName;
    public CloudStorage(string cloudName)
    {
        if (string.IsNullOrWhiteSpace(cloudName))
        {
            throw new ArgumentException("Cloud name cannot be empty. ");
        }
        _cloudName = cloudName;
    }
    public override void Save(string filename, byte[] data)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be empty.");
        }

        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Data cannot be empty");
        }

        Console.WriteLine("Файл: " + filename + " збережено в хмарі: " + _cloudName);
    }
}



class FTPStorage : StorageService
{
    private string _serverAddress;
    public FTPStorage(string serverAddress)
    {
        if (string.IsNullOrWhiteSpace(serverAddress))
        {
            throw new ArgumentException("Server address cannot be empty.");
        }

        _serverAddress = serverAddress;
    }
    public override void Save(string filename, byte[] data)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be empty.");
        }

        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Data cannot be empty.");
        }

        if (_serverAddress == "ftp://server.com")
        {
            throw new InvalidOperationException("Мережа недоступна.");
        }

        Console.WriteLine("Файл: " + filename + " збережено в сервері: " + _serverAddress);
    }
}

class StorageManager
{
    public void SaveAll(List<StorageService> items, string filename, byte[] data)
    {
        foreach (StorageService storage in items)
        {
            try
            {
                storage.Save(filename, data);   
            }

            catch (Exception ex)
            {
                Console.WriteLine("Помилка збереження: " + ex.Message);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        List<StorageService> items = new List<StorageService>();

        items.Add(new LocalStorage("C:\\Files"));
        items.Add(new CloudStorage("Google Drive"));
        items.Add(new FTPStorage("ftp://server.com"));

        StorageManager manager = new StorageManager();

        byte[] data = { 1, 2, 3, 4, 5 };

        manager.SaveAll(items, "document.txt", data);

        Console.WriteLine();

        Console.WriteLine("Перевірка обробки помилки:");

        manager.SaveAll(items, "document.txt", null);
    }
}