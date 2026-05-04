using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TracksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TracksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string? genre = null, int? minBpm = null, int? maxBpm = null)
    {
        pageSize = Math.Min(pageSize, 50);
        var query = _db.Tracks.Include(t => t.Artist).AsQueryable();
        if (!string.IsNullOrEmpty(genre)) query = query.Where(t => t.Genre.ToLower() == genre.ToLower());
        if (minBpm.HasValue) query = query.Where(t => t.Bpm >= minBpm.Value);
        if (maxBpm.HasValue) query = query.Where(t => t.Bpm <= maxBpm.Value);
        query = query.OrderBy(t => t.Title);

        var total = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new { Total = total, Page = page, PageSize = pageSize, Data = data });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _db.Tracks.Include(t => t.Artist).FirstOrDefaultAsync(t => t.Id == id);
        return track is null ? NotFound(new { message = "Faixa não encontrada" }) : Ok(track);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Track track)
    {
        var artistExists = await _db.Artists.AnyAsync(a => a.Id == track.ArtistId);
        if (!artistExists) return BadRequest(new { message = "Artista inválido" });
        _db.Tracks.Add(track);
        await _db.SaveChangesAsync();
        return Created($"/tracks/{track.Id}", track);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Track inputTrack)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track is null) return NotFound(new { message = "Faixa não encontrada" });
        var artistExists = await _db.Artists.AnyAsync(a => a.Id == inputTrack.ArtistId);
        if (!artistExists) return BadRequest(new { message = "Artista inválido" });

        track.Title = inputTrack.Title;
        track.Bpm = inputTrack.Bpm;
        track.Genre = inputTrack.Genre;
        track.ArtistId = inputTrack.ArtistId;
        await _db.SaveChangesAsync();
        return Ok(track);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track is null) return NotFound(new { message = "Faixa não encontrada" });
        _db.Tracks.Remove(track);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Faixa removida com sucesso" });
    }
}
