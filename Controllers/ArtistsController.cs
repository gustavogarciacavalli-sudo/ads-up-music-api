using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ArtistsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtistDto>>> GetAll()
    {
        var artists = await _db.Artists
            .Include(a => a.Genre)
            .OrderBy(a => a.Name)
            .ToListAsync();

        var dtos = artists.Select(a => new ArtistDto
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            GenreId = a.GenreId,
            GenreName = a.Genre?.Name
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistDto>> GetById(int id)
    {
        var artist = await _db.Artists
            .Include(a => a.Genre)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (artist == null)
            return NotFound(new { error = $"Artista com ID {id} não encontrado." });

        return Ok(new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Bio = artist.Bio,
            GenreId = artist.GenreId,
            GenreName = artist.Genre?.Name
        });
    }

    [HttpPost]
    public async Task<ActionResult<ArtistDto>> Create([FromBody] ArtistDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome do artista é obrigatório." });

        var genreExists = await _db.Genres.AnyAsync(g => g.Id == dto.GenreId);
        if (!genreExists)
            return BadRequest(new { error = $"Gênero com ID {dto.GenreId} não encontrado." });

        var artist = new Artist
        {
            Name = dto.Name,
            Bio = dto.Bio,
            GenreId = dto.GenreId
        };

        _db.Artists.Add(artist);
        await _db.SaveChangesAsync();

        await _db.Entry(artist).Reference(a => a.Genre).LoadAsync();

        return Created($"/api/artists/{artist.Id}", new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Bio = artist.Bio,
            GenreId = artist.GenreId,
            GenreName = artist.Genre?.Name
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ArtistDto>> Update(int id, [FromBody] ArtistDto dto)
    {
        var artist = await _db.Artists.FindAsync(id);
        if (artist == null)
            return NotFound(new { error = $"Artista com ID {id} não encontrado." });

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome do artista é obrigatório." });

        var genreExists = await _db.Genres.AnyAsync(g => g.Id == dto.GenreId);
        if (!genreExists)
            return BadRequest(new { error = $"Gênero com ID {dto.GenreId} não encontrado." });

        artist.Name = dto.Name;
        artist.Bio = dto.Bio;
        artist.GenreId = dto.GenreId;

        await _db.SaveChangesAsync();

        await _db.Entry(artist).Reference(a => a.Genre).LoadAsync();

        return Ok(new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Bio = artist.Bio,
            GenreId = artist.GenreId,
            GenreName = artist.Genre?.Name
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var artist = await _db.Artists.FindAsync(id);
        if (artist == null)
            return NotFound(new { error = $"Artista com ID {id} não encontrado." });

        _db.Artists.Remove(artist);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
