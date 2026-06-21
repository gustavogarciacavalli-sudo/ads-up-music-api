using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PlaylistsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlaylistDto>>> GetAll()
    {
        var playlists = await _db.Playlists
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var dtos = playlists.Select(p => new PlaylistDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Mood = p.Mood,
            CreatedAt = p.CreatedAt
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlaylistDto>> GetById(int id)
    {
        var playlist = await _db.Playlists.FindAsync(id);
        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {id} não encontrada." });

        return Ok(new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            Mood = playlist.Mood,
            CreatedAt = playlist.CreatedAt
        });
    }

    [HttpPost]
    public async Task<ActionResult<PlaylistDto>> Create([FromBody] PlaylistDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome da playlist é obrigatório." });

        var playlist = new Playlist
        {
            Name = dto.Name,
            Description = dto.Description,
            Mood = dto.Mood,
            CreatedAt = DateTime.UtcNow
        };

        _db.Playlists.Add(playlist);
        await _db.SaveChangesAsync();

        return Created($"/api/playlists/{playlist.Id}", new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            Mood = playlist.Mood,
            CreatedAt = playlist.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PlaylistDto>> Update(int id, [FromBody] PlaylistDto dto)
    {
        var playlist = await _db.Playlists.FindAsync(id);
        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {id} não encontrada." });

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome da playlist é obrigatório." });

        playlist.Name = dto.Name;
        playlist.Description = dto.Description;
        playlist.Mood = dto.Mood;

        await _db.SaveChangesAsync();

        return Ok(new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            Mood = playlist.Mood,
            CreatedAt = playlist.CreatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var playlist = await _db.Playlists.FindAsync(id);
        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {id} não encontrada." });

        // Remove playlist association from tracks
        var tracks = await _db.Tracks.Where(t => t.PlaylistId == id).ToListAsync();
        foreach (var track in tracks)
        {
            track.PlaylistId = null;
        }

        _db.Playlists.Remove(playlist);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/playlists/{playlistId}/tracks/{trackId}
    [HttpPost("{playlistId}/tracks/{trackId}")]
    public async Task<IActionResult> AddTrack(int playlistId, int trackId)
    {
        var playlist = await _db.Playlists.FindAsync(playlistId);
        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {playlistId} não encontrada." });

        var track = await _db.Tracks.FindAsync(trackId);
        if (track == null)
            return NotFound(new { error = $"Música com ID {trackId} não encontrada." });

        track.PlaylistId = playlistId;
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Música '{track.Title}' adicionada à playlist '{playlist.Name}'." });
    }

    // DELETE /api/playlists/{playlistId}/tracks/{trackId}
    [HttpDelete("{playlistId}/tracks/{trackId}")]
    public async Task<IActionResult> RemoveTrack(int playlistId, int trackId)
    {
        var track = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == trackId && t.PlaylistId == playlistId);
        if (track == null)
            return NotFound(new { error = "Música não encontrada nesta playlist." });

        track.PlaylistId = null;
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Música '{track.Title}' removida da playlist." });
    }

    // GET /api/playlists/{playlistId}/summary
    [HttpGet("{playlistId}/summary")]
    public async Task<ActionResult<PlaylistSummaryDto>> GetSummary(int playlistId)
    {
        var playlist = await _db.Playlists
            .Include(p => p.Tracks)
                .ThenInclude(t => t.Artist)
                    .ThenInclude(a => a!.Genre)
            .FirstOrDefaultAsync(p => p.Id == playlistId);

        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {playlistId} não encontrada." });

        var tracks = playlist.Tracks;

        // Parse durations (format "mm:ss") to total seconds for sum
        int totalSeconds = 0;
        foreach (var t in tracks)
        {
            var parts = t.Duration.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[0], out int mins) && int.TryParse(parts[1], out int secs))
            {
                totalSeconds += mins * 60 + secs;
            }
        }
        int totalMins = totalSeconds / 60;
        int remainingSecs = totalSeconds % 60;

        return Ok(new PlaylistSummaryDto
        {
            Name = playlist.Name,
            TrackCount = tracks.Count,
            AverageBpm = tracks.Count > 0 ? Math.Round(tracks.Average(t => t.Bpm), 1) : 0,
            TotalDuration = $"{totalMins}:{remainingSecs:D2}",
            Artists = tracks.Where(t => t.Artist != null).Select(t => t.Artist!.Name).Distinct().ToList(),
            Genres = tracks.Where(t => t.Artist?.Genre != null).Select(t => t.Artist!.Genre!.Name).Distinct().ToList()
        });
    }

    // GET /api/playlists/{playlistId}/tracks
    [HttpGet("{playlistId}/tracks")]
    public async Task<ActionResult<List<TrackDto>>> GetPlaylistTracks(int playlistId)
    {
        var playlist = await _db.Playlists.FindAsync(playlistId);
        if (playlist == null)
            return NotFound(new { error = $"Playlist com ID {playlistId} não encontrada." });

        var tracks = await _db.Tracks
            .Where(t => t.PlaylistId == playlistId)
            .Include(t => t.Artist)
            .OrderBy(t => t.Title)
            .ToListAsync();

        var dtos = tracks.Select(t => new TrackDto
        {
            Id = t.Id,
            Title = t.Title,
            Bpm = t.Bpm,
            Duration = t.Duration,
            ArtistId = t.ArtistId,
            ArtistName = t.Artist?.Name,
            PlaylistId = t.PlaylistId
        }).ToList();

        return Ok(dtos);
    }
}
