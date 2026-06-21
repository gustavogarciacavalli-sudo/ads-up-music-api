namespace BeatFlowApi.DTOs;

public class PlaylistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
