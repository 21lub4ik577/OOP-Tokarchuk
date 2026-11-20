using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public override string ToString() => $"[{Id}] {Title}";
}

public static class TextProcessor
{
    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Enumerable.Empty<string>();
        }

        var matches = Regex.Matches(
            text.ToLowerInvariant(),
            @"[\p{L}\p{N}_]+(?:[#+]*)");

        return matches
            .Select(match => match.Value)
            .Where(word => word.Length > 0);
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        var stopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        return words.Where(word => !stopWords.Contains(word));
    }
}

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public void AddDocument(Document doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        if (!_documents.TryAdd(doc.Id, doc))
        {
            throw new ArgumentException($"Document with Id {doc.Id} already exists.", nameof(doc));
        }

        foreach (string word in TextProcessor.Tokenize(doc.Content))
        {
            if (!_index.TryGetValue(word, out HashSet<int>? documentIds))
            {
                documentIds = new HashSet<int>();
                _index.Add(word, documentIds);
            }

            documentIds.Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        string normalizedWord = NormalizeQueryWord(word);
        if (normalizedWord.Length == 0 ||
            !_index.TryGetValue(normalizedWord, out HashSet<int>? documentIds))
        {
            return new List<Document>();
        }

        return documentIds
            .Where(_documents.ContainsKey)
            .Select(id => _documents[id])
            .OrderBy(document => document.Id)
            .ToList();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        if (words.Length == 0)
        {
            return new List<Document>();
        }

        string[] normalizedWords = words
            .SelectMany(word => TextProcessor.Tokenize(word))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (normalizedWords.Length == 0)
        {
            return new List<Document>();
        }

        if (!_index.TryGetValue(normalizedWords[0], out HashSet<int>? resultIds))
        {
            return new List<Document>();
        }

        HashSet<int> matchingIds = new HashSet<int>(resultIds);

        for (int i = 1; i < normalizedWords.Length; i++)
        {
            if (!_index.TryGetValue(normalizedWords[i], out HashSet<int>? wordIds))
            {
                return new List<Document>();
            }

            matchingIds.IntersectWith(wordIds);
        }

        return matchingIds
            .Where(_documents.ContainsKey)
            .Select(id => _documents[id])
            .OrderBy(document => document.Id)
            .ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        if (words.Length == 0)
        {
            return new List<Document>();
        }

        string[] normalizedWords = words
            .SelectMany(word => TextProcessor.Tokenize(word))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        HashSet<int> matchingIds = new HashSet<int>();

        foreach (string word in normalizedWords)
        {
            if (_index.TryGetValue(word, out HashSet<int>? wordIds))
            {
                matchingIds.UnionWith(wordIds);
            }
        }

        return matchingIds
            .Where(_documents.ContainsKey)
            .Select(id => _documents[id])
            .OrderBy(document => document.Id)
            .ToList();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;

    private static string NormalizeQueryWord(string word)
    {
        return TextProcessor.Tokenize(word).FirstOrDefault() ?? string.Empty;
    }
}

public static class PerformanceTest
{
    private const int Repetitions = 100;

    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        var stopwatch = new Stopwatch();
        string normalizedWord = TextProcessor.Tokenize(searchWord).FirstOrDefault() ?? string.Empty;

        stopwatch.Start();
        List<Document>? linearResults = null;
        for (int i = 0; i < Repetitions; i++)
        {
            linearResults = documents
                .Where(document =>
                    TextProcessor.Tokenize(document.Content).Contains(normalizedWord))
                .ToList();
        }
        stopwatch.Stop();
        double linearTicksPerSearch = stopwatch.ElapsedTicks / (double)Repetitions;

        var index = new InvertedIndex();
        foreach (Document document in documents)
        {
            index.AddDocument(document);
        }

        stopwatch.Restart();
        List<Document>? indexResults = null;
        for (int i = 0; i < Repetitions; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }
        stopwatch.Stop();
        double indexTicksPerSearch = stopwatch.ElapsedTicks / (double)Repetitions;

        Console.WriteLine(
            $"  Linear: {linearTicksPerSearch:F2} ticks  |  " +
            $"Index: {indexTicksPerSearch:F2} ticks  |  " +
            $"Found: {linearResults?.Count ?? 0}/{indexResults?.Count ?? 0}");

        if ((linearResults?.Count ?? 0) != (indexResults?.Count ?? 0))
        {
            Console.WriteLine("  WARNING: search results differ!");
        }
    }
}

public static class Program
{
    public static void Main()
    {
        var documents = LoadBaseDocuments();
        var index = new InvertedIndex();

        foreach (Document document in documents)
        {
            index.AddDocument(document);
        }

        Console.WriteLine($"Loaded {index.DocumentCount} documents");
        Console.WriteLine($"Index contains {index.UniqueWords} unique words");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter search query (or 'exit'): ");
            string? query = Console.ReadLine();

            if (string.Equals(query, "exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Please enter at least one word.\n");
                continue;
            }

            string[] words = query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Console.WriteLine("\n--- Search Results (AND) ---");
            List<Document> results = index.SearchMultipleWords(words);

            foreach (Document document in results)
            {
                string preview = document.Content.Substring(
                    0,
                    Math.Min(100, document.Content.Length));
                Console.WriteLine(document);
                Console.WriteLine($" {preview}...");
            }

            Console.WriteLine($"\nFound {results.Count} documents\n");
        }

        RunPerformanceTests();
    }

    private static List<Document> LoadBaseDocuments()
    {
        return new List<Document>
        {
            new Document
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern object-oriented programming language developed by Microsoft."
            },
            new Document
            {
                Id = 2,
                Title = "Java Basics",
                Content = "Java is a popular programming language known for its portability and performance."
            },
            new Document
            {
                Id = 3,
                Title = "Python Overview",
                Content = "Python is a high-level programming language with simple syntax."
            },
            new Document
            {
                Id = 4,
                Title = "C# Advanced Features",
                Content = "C# supports LINQ, async/await, and many modern programming paradigms."
            },
            new Document
            {
                Id = 5,
                Title = "Comparing Languages",
                Content = "C#, Java, and Python are among the most popular programming languages today."
            }
        };
    }

    private static List<Document> GenerateDocuments(int count)
    {
        List<Document> baseDocuments = LoadBaseDocuments();
        if (count <= baseDocuments.Count)
        {
            return baseDocuments.Take(count).ToList();
        }

        var generated = new List<Document>(count);
        generated.AddRange(baseDocuments);

        string[] topics =
        {
            "software", "database", "network", "computer", "algorithm",
            "programming", "language", "performance", "testing", "cloud"
        };

        for (int i = baseDocuments.Count + 1; i <= count; i++)
        {
            int topic = i % topics.Length;
            int nextTopic = (i * 7) % topics.Length;
            generated.Add(new Document
            {
                Id = i,
                Title = $"Generated Document {i}",
                Content =
                    $"Document {i} describes {topics[topic]} and {topics[nextTopic]} " +
                    $"in modern programming systems for application performance."
            });
        }

        return generated;
    }

    private static void RunPerformanceTests()
    {
        Console.WriteLine("\n--- Performance comparison (100 searches) ---");

        int[] documentCounts = { 10, 100, 1000, 10000 };
        foreach (int count in documentCounts)
        {
            Console.WriteLine($"{count} documents:");
            List<Document> documents = GenerateDocuments(count);
            PerformanceTest.CompareSearchMethods(documents, "programming");
        }
    }
}
