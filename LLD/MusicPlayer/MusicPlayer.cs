using System;
using System.ComponentModel;

namespace LLD.MusicPlayer;

class Song(string name, string artist, TimeSpan durarion)
{
    public string Name { get; }
    public Guid Id { get; } = Guid.NewGuid();
    public string Artist { get; } = artist;
    public TimeSpan Duration { get; } = durarion;
}

class Playlist
{
    public Guid Id { get; }
    public string Name { get; }
    // public readonly List<Song> Songs; 
    //! Don't expose List<Song> directly. Otherwise anyone can do:
    //! playlist.Songs.Add(...)
    //! playlist.Songs.Remove(...)
    //! playlist.Songs.Clear()
    //! This bypasses Playlist's business methods and breaks encapsulation.
    //! 'readonly' means the _songs reference cannot point to another List after initialization. It DOES NOT make the list immutable—you can still Add(), Remove(), etc. inside this class.
    //! Expose IReadOnlyList<Song> so other classes can only read the collection (iterate, access by index, Count) but cannot modify it.
    private readonly List<Song> _songs = new(); //! Internal implementation detail → private field Something callers should access → property
    //! Exception: If you need property features (custom getter/setter logic, attributes, data binding, serialization, etc.), then even a private member may be a property. Otherwise, a field is the conventional choice for internal storage.
    public IReadOnlyList<Song> Songs => _songs;
    public Playlist(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddSong(Song song)
    {
        _songs.Add(song);
    }
    public void RemoveSong(Guid songId)
    {
        _songs.RemoveAll(song => song.Id == songId);
    }
    public Song GetSong(int index)
    {
        if (index < 0 || index >= _songs.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _songs[index];
    }
    public int Count => _songs.Count;
}

public class MusicLibrary
{
    private readonly List<Song> _songs = new();
    private readonly List<Playlist> _playlists = new();
    public IReadOnlyList<Song> Songs => _songs;
    public IReadOnlyList<Playlist> Playlists => _playlists;

    public void AddSong(Song song)
    {
        _songs.Add(song);
    }
    public void RemoveSong(Guid songId)
    {
        _songs.RemoveAll(song => song.Id == songId);
    }

    public Song GetSong(int index)
    {
        if (index < 0 || index >= _songs.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _songs[index];
    }

    public IReadOnlyList<Song> GetAllSongs()
    {
        return Songs;
    }

    public void AddPlaylist(Playlist playlist)
    {
        _playlists.Add(playlist);
    }

    public void RemovePlaylist(Guid playlistId)
    {
        _playlists.RemoveAll(playlist => playlist.Id == playlistId);
    }

    public Playlist GetPlaylist(int index)
    {
        if (index < 0 || index >= _playlists.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _playlists[index];
    }

    public IReadOnlyList<Playlist> GetAllPlaylists()
    {
        return Playlists;
    }
    public int SongCount => _songs.Count;

    public int PlaylistCount => _playlists.Count;
}

public enum PlayerStatus
{
    Playing,
    Paused,
    Stopped
}

public class MusicPlayer
{
    PlayerStatus Status { get; private set; } = PlayerStatus.Stopped;
    int Volume { get; private set; } = 0;
    private const int MaxVolume = 100;
    private const int MinVolume = 0;
    public void TurnOff()
    {
        Status = PlayerStatus.Stopped;
    }

    public void TurnOn()
    {
        Status = PlayerStatus.Playing;
    }

    public void Pause()
    {
        if (Status == PlayerStatus.Stopped)
            throw new Exception("Cannot pause when player is closed");
        Status = PlayerStatus.Paused;
    }

    public void IncreaseVolume()
    {
        if (Volume >= MaxVolume)
            throw new InvalidOperationException($"Max volume cannot be more than {MaxVolume}.");

        Volume++;
    }

    public void DecreaseVolume()
    {
        if (Volume <= MinVolume)
            throw new InvalidOperationException($"Min volume cannot be less than {MinVolume}.");

        Volume--;
    }

    public void Mute()
    {
        Volume = MinVolume;
    }
}