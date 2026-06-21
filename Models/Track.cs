namespace BeatFlowApi.Models;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Bpm { get; set; }
    public string Duration { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }
    public int? PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }
}
