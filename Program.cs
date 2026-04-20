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

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeatFlow API v1");
    c.RoutePrefix = "swagger"; 
});

// --- ESTILOS E SCRIPTS (VIBE CODING 2005) ---
string sharedHead = @"
    <meta charset='utf-8'>
    <style>
        html, body { margin: 0; padding: 0; background-color: #000; overflow-x: hidden; cursor: default; }
        body { font-family: 'Comic Sans MS', cursive, sans-serif; background-image: url('https://web.archive.org/web/20090829035122/http://geocities.com/Heartland/Acres/9371/backgrounds/stars.gif'); display: flex; justify-content: center; }
        .mouse-clone { position: fixed; width: 22px; height: 22px; pointer-events: none; z-index: 9999; animation: cloneFade 0.8s linear forwards; }
        @keyframes cloneFade { 0% { opacity: 0.8; } 100% { opacity: 0; transform: scale(0.5) translateY(20px); } }
        .main-container { display: flex; width: 1000px; padding: 20px; gap: 10px; align-items: stretch; justify-content: center; }
        .sidebar { width: 150px; display: flex; flex-direction: column; gap: 20px; padding: 10px; flex-shrink: 0; }
        .ad-box { border: 2px solid #F00; background: #FFF; color: #000; font-size: 0.7em; text-align: center; padding: 5px; animation: blinker 0.4s linear infinite; font-weight: bold; }
        .center-panel { width: 600px; background-color: #000080; border: 6px outset #FF00FF; padding: 20px; box-shadow: 15px 15px 0px #444; text-align: center; }
        h1 { font-family: 'Times New Roman', serif; font-size: 3em; color: #FF00FF; text-decoration: underline; margin: 0; }
        .rainbow-wave { font-size: 1.8em; font-weight: bold; display: inline-block; margin: 30px 0; white-space: nowrap; }
        .rainbow-wave span { display: inline-block; animation: rainbow-color 2s linear infinite, hypnotic-cobra 1.2s ease-in-out infinite; min-width: 5px; }
        @keyframes rainbow-color { 0% { color: #F00; } 33% { color: #0F0; } 66% { color: #0FF; } 100% { color: #F0F; } }
        @keyframes hypnotic-cobra { 0%, 100% { transform: translateY(15px); } 50% { transform: translateY(-15px); } }
        marquee { background: #008080; color: #FFF; padding: 8px; border: 2px dashed #FFF; margin: 15px 0; }
        .link-container { border: 3px double #FFF; padding: 15px; background: #000; margin: 15px 0; }
        a.old-school, .btn-del { font-size: 1.1em; color: #0F0; text-decoration: none; font-weight: bold; border: 3px solid #F00; padding: 8px; display: inline-block; background: #000; cursor: pointer; }
        a.old-school:hover, .btn-del:hover { color: #F0F; background: #FF0; transform: scale(1.1); }
        .btn-del { color: #FFF; border-color: #FFF; font-size: 0.7em; padding: 2px 5px; }
        .artist-table { width: 100%; border-collapse: collapse; background: #000; color: #0F0; border: 3px inset #FF00FF; margin-top: 20px; }
        .artist-table th { background: #444; color: #FFF; padding: 8px; border: 1px solid #FFF; }
        .artist-table td { padding: 8px; border: 1px solid #333; text-align: left; }
        @keyframes blinker { 50% { opacity: 0; } }
    </style>
";

string sharedScript = @"
    <script>
        const container = document.getElementById('mouse-echo-container');
        const colors = ['red', 'orange', 'yellow', 'green', 'blue', 'violet'];
        let colorIndex = 0;
        document.addEventListener('mousemove', (e) => {
            const clone = document.createElement('div');
            clone.className = 'mouse-clone';
            clone.style.left = e.clientX + 'px'; clone.style.top = e.clientY + 'px';
            const currentColor = colors[colorIndex];
            colorIndex = (colorIndex + 1) % colors.length;
            clone.innerHTML = `<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'><path d='M3 3l7.07 16.97 2.51-7.39 7.39-2.51L3 3z' fill='${currentColor}' stroke='black' stroke-width='1'/></svg>`;
            container.appendChild(clone);
            setTimeout(() => clone.remove(), 1000);
        });
    </script>
";

// --- ROTA: HOME ---
app.MapGet("/", () => {
    long hits = new Random().NextInt64(1000000000, 9999999999);
    string waveText = "BEM VINDO AO MELHOR SITE DE MUSICA DA NET!!!";
    string rainbowHtml = "";
    for (int i = 0; i < waveText.Length; i++) {
        char c = waveText[i];
        rainbowHtml += $"<span style='animation-delay: {i * 0.05}s'>{(c == ' ' ? "&nbsp;" : c.ToString())}</span>";
    }

    return Results.Content($@"
    <html><head>{sharedHead}</head><body>
        <div id='mouse-echo-container'></div>
        <div class='main-container'>
            <div class='sidebar'><div class='ad-box'>GANHE UM MOTOROLA V3!</div></div>
            <div class='center-panel'>
                <h1>🎵 BEATFLOW API 🚀</h1>
                <div class='rainbow-wave'>{rainbowHtml}</div>
                <marquee>*** SEJA BEM VINDO AO MELHOR BACKEND DA ROOVER ***</marquee>
                <div class='link-container'>
                    <a href='/artists-page' class='old-school'>>>> VER ARTISTAS <<<</a>
                    <a href='/swagger' class='old-school'>>> Swagger <<</a>
                </div>
                <div style='background:#000; color:#0F0; padding:10px; margin-top:20px;'>VISITANTE Nº: {hits:N0}</div>
            </div>
            <div class='sidebar'><div class='ad-box' style='background:#0FF;'>SITE SEGURO!</div></div>
        </div>{sharedScript}
    </body></html>", "text/html; charset=utf-8");
});

// --- ROTA: PÁGINA DE ARTISTAS ---
app.MapGet("/artists-page", () => {
    return Results.Content($@"
    <html><head>{sharedHead}</head><body>
        <div id='mouse-echo-container'></div>
        <div class='main-container'>
            <div class='sidebar'><div class='ad-box' style='background:#F0F; color:#FFF;'>TOQUES RBD!</div></div>
            <div class='center-panel'>
                <h1>🎸 ÁREA DE ARTISTAS 🎸</h1>
                <div style='background:#000; padding:15px; border:2px double #FFF;'>
                    <table class='artist-table'>
                        <thead><tr><th>ID</th><th>NOME</th><th>GÊNERO</th><th>AÇÃO</th></tr></thead>
                        <tbody id='tableContent'><tr><td colspan='4'>Carregando...</td></tr></tbody>
                    </table>
                </div>
                <div style='margin-top:20px;'>
                    <a href='/' class='old-school'><< VOLTAR</a>
                    <a href='/swagger' class='old-school'>>> Swagger <<</a>
                </div>
            </div>
            <div class='sidebar'><div class='ad-box'>DANÇA DO SIRI!</div></div>
        </div>
        <script>
            async function load() {{
                const res = await fetch('/artists');
                const data = await res.json();
                document.getElementById('tableContent').innerHTML = data.map(a => 
                    `<tr><td>${{a.id}}</td><td>${{a.name.toUpperCase()}}</td><td>${{a.genre.toUpperCase()}}</td><td><button class='btn-del' onclick='del(${{a.id}})'>[X] EXCLUIR</button></td></tr>`
                ).join('') || '<tr><td colspan=""4"">Nenhum artista cadastrado.</td></tr>';
            }}
            async function del(id) {{
                if(confirm('Apagar artista?')) {{ await fetch('/artists/' + id, {{ method: 'DELETE' }}); load(); }}
            }}
            load();
        </script>{sharedScript}
    </body></html>", "text/html; charset=utf-8");
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