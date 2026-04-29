using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÕES (SERVICES)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=beatflow.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Configuração simples para não dar erro de OpenApiInfo

var app = builder.Build();

// 2. INICIALIZAÇÃO DO BANCO E SWAGGER
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeatFlow API v1");
    c.RoutePrefix = "swagger"; 
});

// --- ROTAS API ---

// GET /artists?page=1&pageSize=10&genre=rock&sort=asc
app.MapGet("/artists", async (
    AppDbContext db,
    int page = 1,
    int pageSize = 10,
    string? genre = null,
    string sort = "asc") =>
{
    // Limita pageSize a no máximo 50
    pageSize = Math.Min(pageSize, 50);
    if (page < 1) page = 1;

    var query = db.Artists.AsQueryable();

    // Filtro por gênero (case-insensitive)
    if (!string.IsNullOrWhiteSpace(genre))
        query = query.Where(a => a.Genre != null && a.Genre.ToLower() == genre.ToLower());

    // Ordenação por nome
    query = sort.ToLower() == "desc"
        ? query.OrderByDescending(a => a.Name)
        : query.OrderBy(a => a.Name);

    var totalCount = await query.CountAsync();

    var data = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        page,
        pageSize,
        totalCount,
        totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
        data
    });
});

app.MapPost("/artists", async (Artist a, AppDbContext db) => { db.Artists.Add(a); await db.SaveChangesAsync(); return Results.Created($"/artists/{a.Id}", a); });
app.MapDelete("/artists/{id}", async (int id, AppDbContext db) => {
    if (await db.Artists.FindAsync(id) is Artist a) { db.Artists.Remove(a); await db.SaveChangesAsync(); return Results.Ok(a); }
    return Results.NotFound();
});

// --- ROTAS API — TRACKS ---

// GET /tracks?artistId=1  — lista todas as tracks (opcionalmente filtrando por artista)
app.MapGet("/tracks", async (AppDbContext db, int? artistId) =>
{
    var query = db.Tracks.AsQueryable();
    if (artistId.HasValue)
        query = query.Where(t => t.ArtistId == artistId.Value);
    return await query.ToListAsync();
});

// GET /tracks/{id}  — busca track por ID
app.MapGet("/tracks/{id}", async (int id, AppDbContext db) =>
{
    var track = await db.Tracks.FindAsync(id);
    return track is not null ? Results.Ok(track) : Results.NotFound(new { error = $"Track com ID {id} não encontrada." });
});

// POST /tracks  — cria nova track, valida que o ArtistId existe
app.MapPost("/tracks", async (Track track, AppDbContext db) =>
{
    var artistExists = await db.Artists.AnyAsync(a => a.Id == track.ArtistId);
    if (!artistExists)
        return Results.BadRequest(new { error = $"Artista com ID {track.ArtistId} não encontrado. Informe um ArtistId válido." });

    db.Tracks.Add(track);
    await db.SaveChangesAsync();
    return Results.Created($"/tracks/{track.Id}", track);
});

// PUT /tracks/{id}  — atualiza uma track existente
app.MapPut("/tracks/{id}", async (int id, Track updated, AppDbContext db) =>
{
    var track = await db.Tracks.FindAsync(id);
    if (track is null)
        return Results.NotFound(new { error = $"Track com ID {id} não encontrada." });

    var artistExists = await db.Artists.AnyAsync(a => a.Id == updated.ArtistId);
    if (!artistExists)
        return Results.BadRequest(new { error = $"Artista com ID {updated.ArtistId} não encontrado." });

    track.Title  = updated.Title;
    track.Bpm    = updated.Bpm;
    track.Genre  = updated.Genre;
    track.ArtistId = updated.ArtistId;

    await db.SaveChangesAsync();
    return Results.Ok(track);
});

// DELETE /tracks/{id}  — remove a track sem afetar o artista
app.MapDelete("/tracks/{id}", async (int id, AppDbContext db) =>
{
    var track = await db.Tracks.FindAsync(id);
    if (track is null)
        return Results.NotFound(new { error = $"Track com ID {id} não encontrada." });

    db.Tracks.Remove(track);
    await db.SaveChangesAsync();
    return Results.Ok(track);
});

app.Run();

// --- MODELOS ---
public class Artist { public int Id { get; set; } public string? Name { get; set; } public string? Genre { get; set; } }
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Artist> Artists => Set<Artist>();
}