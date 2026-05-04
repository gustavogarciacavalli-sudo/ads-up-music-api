using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BeatFlowApi.Data;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TracksController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public TracksController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTracks([FromQuery] int? artistId)
    {
        var query = _db.Tracks.AsQueryable();
        if (artistId.HasValue)
            query = query.Where(t => t.ArtistId == artistId.Value);
        
        var tracks = await query.ToListAsync();
        var tracksDto = _mapper.Map<List<TrackDto>>(tracks);
        return Ok(tracksDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrack(int id)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track == null)
            return NotFound(new { error = $"Track com ID {id} não encontrada." });
        
        var trackDto = _mapper.Map<TrackDto>(track);
        return Ok(trackDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromBody] TrackDto dto)
    {
        var artistExists = await _db.Artists.AnyAsync(a => a.Id == dto.ArtistId);
        if (!artistExists)
            return BadRequest(new { error = $"Artista com ID {dto.ArtistId} não encontrado. Informe um ArtistId válido." });

        var track = _mapper.Map<Track>(dto);
        _db.Tracks.Add(track);
        await _db.SaveChangesAsync();
        
        var createdDto = _mapper.Map<TrackDto>(track);
        return Created($"/tracks/{track.Id}", createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrack(int id, [FromBody] TrackDto updatedDto)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track == null)
            return NotFound(new { error = $"Track com ID {id} não encontrada." });

        var artistExists = await _db.Artists.AnyAsync(a => a.Id == updatedDto.ArtistId);
        if (!artistExists)
            return BadRequest(new { error = $"Artista com ID {updatedDto.ArtistId} não encontrado." });

        _mapper.Map(updatedDto, track);

        await _db.SaveChangesAsync();
        
        var savedDto = _mapper.Map<TrackDto>(track);
        return Ok(savedDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrack(int id)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track == null)
            return NotFound(new { error = $"Track com ID {id} não encontrada." });

        _db.Tracks.Remove(track);
        await _db.SaveChangesAsync();
        
        var deletedDto = _mapper.Map<TrackDto>(track);
        return Ok(deletedDto);
    }
}
