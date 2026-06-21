namespace BeatFlowApi.DTOs;

public class ArtistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public string? GenreName { get; set; }
}
