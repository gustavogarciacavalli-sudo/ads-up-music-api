using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly AppDbContext _db;

    public GenresController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenreDto>>> GetAll()
    {
        var genres = await _db.Genres
            .OrderBy(g => g.Name)
            .ToListAsync();

        var dtos = genres.Select(g => new GenreDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetById(int id)
    {
        var genre = await _db.Genres.FindAsync(id);
        if (genre == null)
            return NotFound(new { error = $"Gênero com ID {id} não encontrado." });

        return Ok(new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        });
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create([FromBody] GenreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome do gênero é obrigatório." });

        var genre = new Genre
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _db.Genres.Add(genre);
        await _db.SaveChangesAsync();

        return Created($"/api/genres/{genre.Id}", new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GenreDto>> Update(int id, [FromBody] GenreDto dto)
    {
        var genre = await _db.Genres.FindAsync(id);
        if (genre == null)
            return NotFound(new { error = $"Gênero com ID {id} não encontrado." });

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Nome do gênero é obrigatório." });

        genre.Name = dto.Name;
        genre.Description = dto.Description;

        await _db.SaveChangesAsync();

        return Ok(new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _db.Genres.FindAsync(id);
        if (genre == null)
            return NotFound(new { error = $"Gênero com ID {id} não encontrado." });

        _db.Genres.Remove(genre);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
