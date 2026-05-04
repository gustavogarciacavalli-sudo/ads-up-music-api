namespace BeatFlowApi.DTOs;

public class TrackDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Bpm { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int ArtistId { get; set; }
}
