namespace BeatFlowApi.DTOs;

public class PlaylistSummaryDto
{
    public string Name { get; set; } = string.Empty;
    public int TrackCount { get; set; }
    public double AverageBpm { get; set; }
    public string TotalDuration { get; set; } = string.Empty;
    public List<string> Artists { get; set; } = new();
    public List<string> Genres { get; set; } = new();
}
