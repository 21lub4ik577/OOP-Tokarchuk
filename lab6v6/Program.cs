using System;

class MediaItem
{
    private string _title;
    private int _duration;

    public string Title
    {
        get {return _title;}
        set {_title = value;}
    }

    public int Duration
    {
        get {return _duration;}
        set {_duration = value;}
    }

    public MediaItem(string title, int duration)
    {
        _title = title;
        _duration = duration;
    }

    public virtual void Play()
    {
        Console.WriteLine("playing: " + Title);
    }

        public string GetMediaType()
    {
        return "Media";
    }
}

class BookMedia : MediaItem
{
    private string _author;

    public string Author
    {
        get {return _author;}
        set {_author = value;}
    }

    public override void Play()
    {
        Console.WriteLine("Book: " + Title + " From the Author: " + Author);
    }

    public void ReadSample()
    {
        Console.WriteLine("Reading a sample of the book");
    }

    public BookMedia(string title, int duration, string author) : base(title, duration)
    {
        _author = author;
    }

    public new string GetMediaType()
    {
        return "Book";
    }
}

class MovieMedia : MediaItem
{
    private string _director;

    public string Director
    {
        get {return _director;}
        set {_director = value;}
    }

    public override void Play()
    {
        Console.WriteLine("Movie: " + Title + " From the director: " + Director);
    }

    public void ShowTrailer()
    {
        Console.WriteLine("Watching the trailer of the movie");
    }

    public MovieMedia(string title, int duration, string director) : base(title, duration)
    {
        _director = director;
    }

    public new string GetMediaType()
    {
        return "Movie";
    }
}

class Program
{
    static void Main()
    {
        BookMedia book = new BookMedia("It", 120, "Stephen King");
        MovieMedia movie = new MovieMedia("Interstellar", 169, "Christopher Nolan");

        MediaItem mediaBook = book;
        mediaBook.Play();

        MediaItem mediaMovie = movie;
        mediaMovie.Play();

        book.ReadSample();
        movie.ShowTrailer();

    Console.WriteLine("BookMedia: " + book.GetMediaType());
    Console.WriteLine("MediaItem reference: " + mediaBook.GetMediaType());
    Console.WriteLine("MovieMedia: " + movie.GetMediaType());
    }
}