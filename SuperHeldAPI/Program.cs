global using Microsoft.EntityFrameworkCore;
global using SuperHeldAPI.Datenzugriff;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//! Datenbankkontext konfigurieren
builder.Services.AddDbContext<DatenzugriffKontext>(options =>
{
    //! Passwort aus Umgebungsvariable abrufen
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    if (string.IsNullOrEmpty(dbPassword))
    {
        throw new InvalidOperationException(
            "Das Datenbankkennwort wurde in der Umgebungsvariablen nicht gefunden."
        );
    }

    //! Hole sich den Verbindungszeichenfolge aus der Datei appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException(
            "Verbindungszeichenfolge „DefaultConnection“ nicht gefunden."
        );
    }

    //! Ersetze den Platzhalter {DB_PASSWORD} in der Verbindungszeichenfolge durch das tatsächliche Passwort
    connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
    options.UseSqlServer(connectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperHeldAPI v1");
        c.RoutePrefix = string.Empty; //! Swagger UI unter der Stamm-URL hosten
    });
    //! Öffne die Swagger-URL im Standardbrowser, nachdem die Anwendung gestartet wurde
    var url = "https://localhost:7140"; //! oder http://localhost:5140
    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); // Swagger UI'yi kök URL'de barındırmak için