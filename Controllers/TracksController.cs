using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TracksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<TrackDto>>> GetAll()
    {
        var tracks = await _db.Tracks
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

    [HttpGet("{id}")]
    public async Task<ActionResult<TrackDto>> GetById(int id)
    {
        var track = await _db.Tracks
            .Include(t => t.Artist)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (track == null)
            return NotFound(new { error = $"Música com ID {id} não encontrada." });

        return Ok(new TrackDto
        {
            Id = track.Id,
            Title = track.Title,
            Bpm = track.Bpm,
            Duration = track.Duration,
            ArtistId = track.ArtistId,
            ArtistName = track.Artist?.Name,
            PlaylistId = track.PlaylistId
        });
    }

    [HttpPost]
    public async Task<ActionResult<TrackDto>> Create([FromBody] TrackDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { error = "Título da música é obrigatório." });

        if (dto.Bpm <= 0)
            return BadRequest(new { error = "BPM deve ser um valor positivo." });

        var artistExists = await _db.Artists.AnyAsync(a => a.Id == dto.ArtistId);
        if (!artistExists)
            return BadRequest(new { error = $"Artista com ID {dto.ArtistId} não encontrado." });

        var track = new Track
        {
            Title = dto.Title,
            Bpm = dto.Bpm,
            Duration = dto.Duration,
            ArtistId = dto.ArtistId,
            PlaylistId = dto.PlaylistId
        };

        _db.Tracks.Add(track);
        await _db.SaveChangesAsync();

        await _db.Entry(track).Reference(t => t.Artist).LoadAsync();

        return Created($"/api/tracks/{track.Id}", new TrackDto
        {
            Id = track.Id,
            Title = track.Title,
            Bpm = track.Bpm,
            Duration = track.Duration,
            ArtistId = track.ArtistId,
            ArtistName = track.Artist?.Name,
            PlaylistId = track.PlaylistId
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TrackDto>> Update(int id, [FromBody] TrackDto dto)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track == null)
            return NotFound(new { error = $"Música com ID {id} não encontrada." });

        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { error = "Título da música é obrigatório." });

        if (dto.Bpm <= 0)
            return BadRequest(new { error = "BPM deve ser um valor positivo." });

        var artistExists = await _db.Artists.AnyAsync(a => a.Id == dto.ArtistId);
        if (!artistExists)
            return BadRequest(new { error = $"Artista com ID {dto.ArtistId} não encontrado." });

        track.Title = dto.Title;
        track.Bpm = dto.Bpm;
        track.Duration = dto.Duration;
        track.ArtistId = dto.ArtistId;
        track.PlaylistId = dto.PlaylistId;

        await _db.SaveChangesAsync();

        await _db.Entry(track).Reference(t => t.Artist).LoadAsync();

        return Ok(new TrackDto
        {
            Id = track.Id,
            Title = track.Title,
            Bpm = track.Bpm,
            Duration = track.Duration,
            ArtistId = track.ArtistId,
            ArtistName = track.Artist?.Name,
            PlaylistId = track.PlaylistId
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track == null)
            return NotFound(new { error = $"Música com ID {id} não encontrada." });

        _db.Tracks.Remove(track);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
