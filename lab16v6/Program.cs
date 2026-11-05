using System;
using System.Collections.Generic;
using System.Linq;

class Playlist
{
    public int Id;
    public string Name = "";
    public List<Song> List = new();
}

class Song
{
    public int Id;
    public string Title = "";

    public decimal DurationSeconds;
    
    public Artist Artist = new();
    
}

class Artist
{
    public int Id;
    public string Name = "";
    
}

class Program
{
    static void Main()
    {
        List<Playlist> playlists = new()
        {
            new Playlist{Id = 1, Name = "Rock songs", List = new()
            {
                new Song { Id = 1, Title = "Welcome to the jungle", DurationSeconds = 180, Artist = new Artist { Id = 1, Name = "Guns N' Roses" } },
                new Song { Id = 2, Title = "Come as you are", DurationSeconds = 200, Artist = new Artist { Id = 2, Name = "Nirvana" } },
                new Song { Id = 3, Title = "Psychosocial", DurationSeconds = 210, Artist = new Artist { Id = 3, Name = "Slipknot" } },
                new Song { Id = 4, Title = "Nightrain", DurationSeconds = 190, Artist = new Artist { Id = 1, Name = "Guns N' Roses" } }
            }},

            new Playlist{Id = 2, Name = "Pop songs", List = new()
            {
                new Song { Id = 5, Title = "Blinding Lights", DurationSeconds = 180, Artist = new Artist { Id = 5, Name = "The Weeknd" } },
                new Song { Id = 6, Title = "Shape of You", DurationSeconds = 230, Artist = new Artist { Id = 6, Name = "Ed Sheeran" } },
                new Song { Id = 7, Title = "Bad Romance", DurationSeconds = 220, Artist = new Artist { Id = 7, Name = "Lady Gaga" } },
                new Song { Id = 8, Title = "Starboy", DurationSeconds = 210, Artist = new Artist { Id = 5, Name = "The Weeknd" } }
            }},

            new Playlist{Id = 3, Name = "Electronic music", List = new()
            {
                new Song { Id = 9, Title = "Strobe", DurationSeconds = 300, Artist = new Artist { Id = 8, Name = "deadmau5" } },
                new Song { Id = 10, Title = "Animals", DurationSeconds = 215, Artist = new Artist { Id = 9, Name = "Martin Garrix" } },
                new Song { Id = 11, Title = "Titanium", DurationSeconds = 245, Artist = new Artist { Id = 10, Name = "David Guetta" } },
                new Song { Id = 12, Title = "Ghosts 'n' Stuff", DurationSeconds = 210, Artist = new Artist { Id = 8, Name = "deadmau5" } }
            }}

        };

        // Унікальні виконавці
        var artists = playlists.SelectMany(p => p.List).Select(s => s.Artist).DistinctBy(a => a.Id);
        foreach (Artist artist in artists)
        {
            Console.WriteLine(artist.Name);
        }

        // Пагінація
        int pageSize = 5;
        var allSongs = playlists.SelectMany(p => p.List).ToList();
        for (int page = 0; page < allSongs.Count; page += pageSize)
        {
            Console.WriteLine($"\n Page - {page / pageSize + 1}:");
            foreach (Song song in allSongs.Skip(page).Take(pageSize))
                {
                    Console.WriteLine(song.Title);
                }
        }

        // Загальна тривалість
        Console.WriteLine($"\nTotal duration: {allSongs.TotalDuration()} seconds");

    }
}

static class SongExtensions
{
    public static decimal TotalDuration(this IEnumerable<Song> songs)
    {
        return songs.Sum(s => s.DurationSeconds);
    }
}
