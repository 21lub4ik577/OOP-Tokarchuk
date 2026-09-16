using System;
class AudioPlayer: IDisposable
{
    private string _trackName;
    private bool _isPlaying;
    private bool _disposed = false;

    private bool _isResourceAllocated;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Disposing managed resources");
            }
            if (_isResourceAllocated)
            {
                Console.WriteLine("Releasing unmanaged resource");
                _isResourceAllocated = false;
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~AudioPlayer()
    {
        Dispose(false);
    }
    


    public void Play()
    {
        _isPlaying = true;
    }

    public void Stop()
    {
        _isPlaying = false;
    }

    public string TrackName
    {
        get{return _trackName;}
        set{_trackName = value;}
    }

    public bool IsPlaying
    {
        get{return _isPlaying;}
        set{_isPlaying = value;}
    }

    public AudioPlayer(string trackName)
    {
        _trackName = trackName;
        IsPlaying = false;
        _isResourceAllocated = true;
    }
}

class Program
{
    static void CreatePlayerWithoutDispose()
    {
        var player3 = new AudioPlayer("Song 3");
        player3.Play();
    }
    static void Main()
    {
        using (var player = new AudioPlayer("Song 1")){ player.Play();}

        var player2 = new AudioPlayer("Song 2");
        player2.Play();
        player2.Dispose();

        CreatePlayerWithoutDispose();

        
        GC.Collect();
        GC.WaitForPendingFinalizers();

    }
}