
using System;
using System.Collections.Generic;

// Сценарій 1: Кеш метаданих (Dictionary)
public class FileMetadataCache
{
    private Dictionary<string, FileMetadata> _cache;

    public FileMetadataCache()
    {
        _cache = new Dictionary<string, FileMetadata>();
    }

    public void Add(string key, FileMetadata metadata)
    {
        _cache.Add(key, metadata);
    }

    public FileMetadata Get(string key)
    {
        return _cache[key];
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public bool Contains(string key)
    {
        return _cache.ContainsKey(key);
    }
}

public class FileMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = string.Empty;
}


// Сценарій 2: Унікальні теги (HashSet)
public class TagManager
{
    private Dictionary<string, HashSet<string>> _articleTags;

    public TagManager()
    {
        _articleTags = new Dictionary<string, HashSet<string>>();
    }

    public void AddTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            _articleTags.Add(articleId, new HashSet<string>());
        }

        _articleTags[articleId].Add(tag);
    }

    public void RemoveTag(string articleId, string tag)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            return;
        }

        _articleTags[articleId].Remove(tag);
    }

    public HashSet<string> GetAllUniqueTags()
    {
        HashSet<string> allTags = new HashSet<string>();

        foreach (var tags in _articleTags.Values)
        {
            allTags.UnionWith(tags);
        }

        return allTags;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        if (!_articleTags.ContainsKey(articleId1) ||
            !_articleTags.ContainsKey(articleId2))
        {
            return new HashSet<string>();
        }

        HashSet<string> commonTags =
            new HashSet<string>(_articleTags[articleId1]);

        commonTags.IntersectWith(_articleTags[articleId2]);

        return commonTags;
    }

    public HashSet<string> GetUniqueTagsForArticle(string articleId)
    {
        if (!_articleTags.ContainsKey(articleId))
        {
            return new HashSet<string>();
        }

        HashSet<string> uniqueTags = GetAllUniqueTags();
        uniqueTags.ExceptWith(_articleTags[articleId]);

        return uniqueTags;
    }
}


// Сценарій 3: Індекс для пошуку (Dictionary + HashSet)
public class SearchIndex
{
    private Dictionary<string, HashSet<int>> _invertedIndex;

    public SearchIndex()
    {
        _invertedIndex =
            new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
    }

    public void AddDocument(int docId, string content)
    {
        string[] words = content.Split(
            new[] { ' ', '\t', '\n', '\r' },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (!_invertedIndex.ContainsKey(word))
            {
                _invertedIndex.Add(word, new HashSet<int>());
            }

            _invertedIndex[word].Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        if (!_invertedIndex.ContainsKey(word))
        {
            return new HashSet<int>();
        }

        return new HashSet<int>(_invertedIndex[word]);
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
        {
            return new HashSet<int>();
        }

        HashSet<int> result = Search(words[0]);

        for (int i = 1; i < words.Length; i++)
        {
            result.IntersectWith(Search(words[i]));
        }

        return result;
    }
}


// Головна програма
public class Program
{
    public static void Main()
    {
        // Сценарій 1: FileMetadataCache
        Console.WriteLine("=== Кеш метаданих файлів ===");

        FileMetadataCache cache = new FileMetadataCache();

        cache.Add("file1", new FileMetadata
        {
            FileName = "document.pdf",
            Size = 2048,
            LastModified = DateTime.Now,
            Hash = "abc123"
        });

        Console.WriteLine($"Файл існує: {cache.Contains("file1")}");

        FileMetadata metadata = cache.Get("file1");

        Console.WriteLine($"Назва: {metadata.FileName}");
        Console.WriteLine($"Розмір: {metadata.Size} байт");
        Console.WriteLine($"Хеш: {metadata.Hash}");

        cache.Remove("file1");

        Console.WriteLine($"Після видалення: {cache.Contains("file1")}");


        // Сценарій 2: TagManager
        Console.WriteLine("\n=== Унікальні теги ===");

        TagManager tagManager = new TagManager();

        tagManager.AddTag("article1", "CSharp");
        tagManager.AddTag("article1", "Programming");
        tagManager.AddTag("article1", "OOP");

        tagManager.AddTag("article2", "CSharp");
        tagManager.AddTag("article2", "Collections");
        tagManager.AddTag("article2", "Programming");

        Console.WriteLine(
            $"Усі унікальні теги: {string.Join(", ", tagManager.GetAllUniqueTags())}");

        Console.WriteLine(
            $"Спільні теги: {string.Join(", ", tagManager.FindCommonTags("article1", "article2"))}");

        Console.WriteLine(
            $"Теги, яких немає в article1: {string.Join(", ", tagManager.GetUniqueTagsForArticle("article1"))}");

        tagManager.RemoveTag("article1", "OOP");

        Console.WriteLine(
            $"Усі теги після видалення: {string.Join(", ", tagManager.GetAllUniqueTags())}");


        // Сценарій 3: SearchIndex
        Console.WriteLine("\n=== Пошуковий індекс ===");

        SearchIndex searchIndex = new SearchIndex();

        searchIndex.AddDocument(1, "CSharp is a programming language");
        searchIndex.AddDocument(2, "CSharp supports object oriented programming");
        searchIndex.AddDocument(3, "Java is a programming language");

        Console.WriteLine(
            $"Документи зі словом CSharp: {string.Join(", ", searchIndex.Search("CSharp"))}");

        Console.WriteLine(
            $"Документи зі словами CSharp і programming: {string.Join(", ", searchIndex.SearchMultiple("CSharp", "programming"))}");
    }
}
