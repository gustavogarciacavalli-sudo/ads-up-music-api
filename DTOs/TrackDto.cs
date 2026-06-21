namespace BeatFlowApi.DTOs;

public class TrackDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Bpm { get; set; }
    public string Duration { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public int? PlaylistId { get; set; }
}
