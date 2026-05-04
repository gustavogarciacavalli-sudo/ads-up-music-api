using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ArtistsController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtists(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? genre = null,
        [FromQuery] string sort = "asc")
    {
        pageSize = Math.Min(pageSize, 50);
        if (page < 1) page = 1;

        var query = _db.Artists.AsQueryable();

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(a => a.Genre != null && a.Genre.ToLower() == genre.ToLower());

        query = sort.ToLower() == "desc"
            ? query.OrderByDescending(a => a.Name)
            : query.OrderBy(a => a.Name);

        var totalCount = await query.CountAsync();

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dataDto = _mapper.Map<List<ArtistDto>>(data);

        return Ok(new
        {
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            data = dataDto
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateArtist([FromBody] ArtistDto dto)
    {
        var artist = _mapper.Map<Artist>(dto);
        _db.Artists.Add(artist);
        await _db.SaveChangesAsync();
        
        var createdDto = _mapper.Map<ArtistDto>(artist);
        return Created($"/artists/{artist.Id}", createdDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        if (await _db.Artists.FindAsync(id) is Artist a)
        {
            _db.Artists.Remove(a);
            await _db.SaveChangesAsync();
            
            var deletedDto = _mapper.Map<ArtistDto>(a);
            return Ok(deletedDto);
        }
        return NotFound();
    }
}
