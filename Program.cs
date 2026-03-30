using Microsoft.EntityFrameworkCore;
using BeatFlowApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=beatflow.db"));

var app = builder.Build();

// Cria o banco automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ARTISTS
app.MapGet("/artists", async (AppDbContext db) =>
    await db.Artists.ToListAsync());

app.MapGet("/artists/{id}", async (int id, AppDbContext db) =>
    await db.Artists.FindAsync(id) is Artist a ? Results.Ok(a) : Results.NotFound());

app.MapPost("/artists", async (Artist artist, AppDbContext db) => {
    db.Artists.Add(artist);
    await db.SaveChangesAsync();
    return Results.Created($"/artists/{artist.Id}", artist);
});

app.MapPut("/artists/{id}", async (int id, Artist input, AppDbContext db) => {
    var artist = await db.Artists.FindAsync(id);
    if (artist is null) return Results.NotFound();
    artist.Name = input.Name;
    artist.Bio = input.Bio;
    await db.SaveChangesAsync();
    return Results.Ok(artist);
});

app.MapDelete("/artists/{id}", async (int id, AppDbContext db) => {
    var artist = await db.Artists.FindAsync(id);
    if (artist is null) return Results.NotFound();
    db.Artists.Remove(artist);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// TRACKS
app.MapGet("/tracks", async (AppDbContext db) =>
    await db.Tracks.ToListAsync());

app.MapPost("/tracks", async (Track track, AppDbContext db) => {
    db.Tracks.Add(track);
    await db.SaveChangesAsync();
    return Results.Created($"/tracks/{track.Id}", track);
});

app.MapPut("/tracks/{id}", async (int id, Track input, AppDbContext db) => {
    var track = await db.Tracks.FindAsync(id);
    if (track is null) return Results.NotFound();
    track.Title = input.Title;
    track.Bpm = input.Bpm;
    track.Genre = input.Genre;
    track.ArtistId = input.ArtistId;
    await db.SaveChangesAsync();
    return Results.Ok(track);
});

app.MapDelete("/tracks/{id}", async (int id, AppDbContext db) => {
    var track = await db.Tracks.FindAsync(id);
    if (track is null) return Results.NotFound();
    db.Tracks.Remove(track);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();