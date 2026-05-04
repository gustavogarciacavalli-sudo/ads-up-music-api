namespace BeatFlowApi.Models;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Bpm { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }
}
