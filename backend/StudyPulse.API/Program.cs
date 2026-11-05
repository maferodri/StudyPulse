using DotNetEnv;
using StudyPulse.Infrastructure;
using StudyPulse.Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")
                       ?? throw new InvalidOperationException("Falta la cadena de conexión");

var corsAllowedOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS")
                         ?? throw new InvalidOperationException(
                             "Falta la variable CORS_ALLOWED_ORIGINS en el archivo .env.");

//Son los archivos creados en DependencyInjection.cs
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCorsStudyPulse", policy =>
    {
        // Separamos por coma por si existen múltiples orígenes (ej. localhost + https://*.unah.edu.hn)
        var origenes = corsAllowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        policy.WithOrigins(origenes)
            .SetIsOriginAllowedToAllowWildcardSubdomains() // Habilita comodines de subdominio (ej. https://*.unah.edu.hn)
            .AllowAnyHeader()                              // Autoriza encabezados JSON y futuros Tokens JWT
            .AllowAnyMethod();                             // Autoriza verbos GET y Preflights (POST, PUT, DELETE)
    });
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("PoliticaCorsStudyPulse");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.Run();
