using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;
using BeatFlowApi.Models;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ArtistsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string? genre = null, string sort = "asc")
    {
        pageSize = Math.Min(pageSize, 50);
        var query = _db.Artists.AsQueryable();
        if (!string.IsNullOrEmpty(genre)) query = query.Where(a => a.Genre.ToLower() == genre.ToLower());
        query = sort.ToLower() == "desc" ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name);
        var total = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new { Total = total, Page = page, PageSize = pageSize, Data = data });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var artist = await _db.Artists.FindAsync(id);
        return artist is null ? NotFound(new { message = "Artista não encontrado" }) : Ok(artist);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Artist artist)
    {
        _db.Artists.Add(artist);
        await _db.SaveChangesAsync();
        return Created($"/artists/{artist.Id}", artist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Artist inputArtist)
    {
        var artist = await _db.Artists.FindAsync(id);
        if (artist is null) return NotFound(new { message = "Artista não encontrado" });
        artist.Name = inputArtist.Name;
        artist.Bio = inputArtist.Bio;
        artist.Genre = inputArtist.Genre;
        await _db.SaveChangesAsync();
        return Ok(artist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var artist = await _db.Artists.FindAsync(id);
        if (artist is null) return NotFound(new { message = "Artista não encontrado" });
        _db.Artists.Remove(artist);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Artista removido com sucesso" });
    }
}
