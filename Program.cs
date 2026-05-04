using Microsoft.EntityFrameworkCore;
using BeatFlowApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÕES (SERVICES)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=beatflow.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();

// 2. MIDDLEWARES
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Cria banco de dados de teste
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();