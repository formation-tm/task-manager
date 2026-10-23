using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Context;
using TaskManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // Nécessaire pour détecter les endpoints
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MyScopedService>();
builder.Services.AddSingleton<MySingletonService>();

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
     .LogTo(Console.WriteLine, LogLevel.Information)
     .EnableSensitiveDataLogging());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Génère l'interface graphique
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
