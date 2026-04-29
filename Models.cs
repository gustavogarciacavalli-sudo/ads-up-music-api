namespace BeatFlowApi;

/// <summary>
/// Representa um artista no sistema.
/// </summary>
public class Artist
{
    /// <summary>Identificador único do artista.</summary>
    public int Id { get; set; }
    /// <summary>Nome do artista.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Biografia do artista.</summary>
    public string Bio { get; set; } = string.Empty;
    /// <summary>Lista de faixas do artista.</summary>
    public List<Track> Tracks { get; set; } = new();
}

/// <summary>
/// Representa uma música/faixa de um artista.
/// </summary>
public class Track
{
    /// <summary>Identificador único da faixa.</summary>
    public int Id { get; set; }
    /// <summary>Título da faixa.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Batidas por minuto (BPM).</summary>
    public int Bpm { get; set; }
    /// <summary>Gênero musical da faixa.</summary>
    public string Genre { get; set; } = string.Empty;
    /// <summary>Identificador do artista da faixa.</summary>
    public int ArtistId { get; set; }
    /// <summary>Referência ao artista.</summary>
    public Artist? Artist { get; set; }
}