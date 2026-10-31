using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int DirectorId { get; set; }
    public int ReleaseYear { get; set; }
}

class Director
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Country { get; set; } = "";
}

class Program
{
    static void Main()
    {
        List<Director> directors = new List<Director>
        {
            new Director { Id = 1, Name = "Christopher Nolan", Country = "UK" },
            new Director { Id = 2, Name = "Steven Spielberg", Country = "USA" },
            new Director { Id = 3, Name = "James Cameron", Country = "Canada" },
            new Director { Id = 4, Name = "Peter Jackson", Country = "New Zealand" },
            new Director { Id = 5, Name = "Quentin Tarantino", Country = "USA" },
            new Director { Id = 6, Name = "Martin Scorsese", Country = "USA" },
            new Director { Id = 7, Name = "Ridley Scott", Country = "UK" },
            new Director { Id = 8, Name = "David Fincher", Country = "USA" },
            new Director { Id = 9, Name = "Denis Villeneuve", Country = "Canada" },
            new Director { Id = 10, Name = "Greta Gerwig", Country = "USA" }
        };

        List<Movie> movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Memento", DirectorId = 1, ReleaseYear = 2000 },
            new Movie { Id = 2, Title = "The Dark Knight", DirectorId = 1, ReleaseYear = 2008 },
            new Movie { Id = 3, Title = "Inception", DirectorId = 1, ReleaseYear = 2010 },
            new Movie { Id = 4, Title = "Interstellar", DirectorId = 1, ReleaseYear = 2014 },
            new Movie { Id = 5, Title = "Oppenheimer", DirectorId = 1, ReleaseYear = 2023 },
            new Movie { Id = 6, Title = "Jurassic Park", DirectorId = 2, ReleaseYear = 1993 },
            new Movie { Id = 7, Title = "Avatar", DirectorId = 3, ReleaseYear = 2009 },
            new Movie { Id = 8, Title = "The Lord of the Rings", DirectorId = 4, ReleaseYear = 2001 },
            new Movie { Id = 9, Title = "Pulp Fiction", DirectorId = 5, ReleaseYear = 1994 },
            new Movie { Id = 10, Title = "The Irishman", DirectorId = 6, ReleaseYear = 2019 },
            new Movie { Id = 11, Title = "Gladiator", DirectorId = 7, ReleaseYear = 2000 },
            new Movie { Id = 12, Title = "Fight Club", DirectorId = 8, ReleaseYear = 1999 },
            new Movie { Id = 13, Title = "Dune", DirectorId = 9, ReleaseYear = 2021 },
            new Movie { Id = 14, Title = "Barbie", DirectorId = 10, ReleaseYear = 2023 },
            new Movie { Id = 15, Title = "Dunkirk", DirectorId = 1, ReleaseYear = 2017 }
        };

        Console.WriteLine($"Directors: {directors.Count}, Movies: {movies.Count}");

        Console.WriteLine("\nTASK 1: Movies by Christopher Nolan after 2000");
        Stopwatch sw = Stopwatch.StartNew();

        var query1 =
            from movie in movies
            join dir in directors on movie.DirectorId equals dir.Id
            where dir.Name == "Christopher Nolan" && movie.ReleaseYear > 2000
            select movie;
        var result1 = query1.ToList();
        sw.Stop();
        long query1Time = sw.ElapsedTicks;

        foreach (var movie in result1)
            Console.WriteLine($"{movie.Title} ({movie.ReleaseYear})");

        Console.WriteLine($"Query syntax: {query1Time} ticks");

        sw.Restart();

        var result1Method = movies
            .Where(m => m.ReleaseYear > 2000)
            .Join(directors.Where(d => d.Name == "Christopher Nolan"),
                m => m.DirectorId,
                d => d.Id,
                (m, d) => m)
            .ToList();

        sw.Stop();
        long query1MethodTime = sw.ElapsedTicks;
        Console.WriteLine($"Method syntax: {query1MethodTime} ticks");

        Console.WriteLine("\nTASK 2: Movies and their directors");

        sw.Restart();

        var query2 =
            from movie in movies
            join dir in directors on movie.DirectorId equals dir.Id
            select new { movie.Title, DirectorName = dir.Name };

        var result2 = query2.ToList();
        sw.Stop();
        long query2Time = sw.ElapsedTicks;

        foreach (var item in result2)
            Console.WriteLine($"{item.Title} - {item.DirectorName}");

        Console.WriteLine($"Query syntax: {query2Time} ticks");

        sw.Restart();

        var result2Method = movies
            .Join(directors,
                m => m.DirectorId,
                d => d.Id,
                (m, d) => new { m.Title, DirectorName = d.Name })
            .ToList();

        sw.Stop();
        long query2MethodTime = sw.ElapsedTicks;
        Console.WriteLine($"Method syntax: {query2MethodTime} ticks");

        Console.WriteLine("\nTASK 3: Number of movies per director");

        sw.Restart();

        var query3 =
            from movie in movies
            join dir in directors on movie.DirectorId equals dir.Id
            group movie by dir.Name into grouped
            select new
            {
                DirectorName = grouped.Key,
                MovieCount = grouped.Count()
            };

        var result3 = query3.ToList();
        sw.Stop();
        long query3Time = sw.ElapsedTicks;

        foreach (var item in result3)
            Console.WriteLine($"{item.DirectorName}: {item.MovieCount}");

        Console.WriteLine($"Query syntax: {query3Time} ticks");

        sw.Restart();

        var result3Method = movies
            .Join(directors,
                m => m.DirectorId,
                d => d.Id,
                (m, d) => new { d.Name, Movie = m })
            .GroupBy(x => x.Name)
            .Select(g => new
            {
                DirectorName = g.Key,
                MovieCount = g.Count()
            })
            .ToList();

        sw.Stop();
        long query3MethodTime = sw.ElapsedTicks;
        Console.WriteLine($"Method syntax: {query3MethodTime} ticks");

        Console.WriteLine("\nTASK 4: Newest movie");

        sw.Restart();

        var query4 =
            (from movie in movies
            orderby movie.ReleaseYear descending
            select movie).First();

        sw.Stop();
        long query4Time = sw.ElapsedTicks;

        Console.WriteLine($"{query4.Title} ({query4.ReleaseYear})");
        Console.WriteLine($"Query syntax: {query4Time} ticks");

        sw.Restart();

        var query4Method = movies
            .OrderByDescending(m => m.ReleaseYear)
            .First();

        sw.Stop();
        long query4MethodTime = sw.ElapsedTicks;

        Console.WriteLine($"{query4Method.Title} ({query4Method.ReleaseYear})");
        Console.WriteLine($"Method syntax: {query4MethodTime} ticks");

        Console.WriteLine("\nPERFORMANCE SUMMARY");
        Console.WriteLine($"Task 1: query = {query1Time}, method = {query1MethodTime} ticks");
        Console.WriteLine($"Task 2: query = {query2Time}, method = {query2MethodTime} ticks");
        Console.WriteLine($"Task 3: query = {query3Time}, method = {query3MethodTime} ticks");
        Console.WriteLine($"Task 4: query = {query4Time}, method = {query4MethodTime} ticks");
    }
}
