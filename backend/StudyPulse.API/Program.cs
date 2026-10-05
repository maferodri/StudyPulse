using DotNetEnv;
using StudyPulse.Infrastructure;
using StudyPulse.Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")
                       ?? throw new InvalidOperationException("Falta la cadena de conexión");

//Son los archivos creados en DependencyInjection.cs
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddControllers();
builder.Services.AddApplication();

builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.Run();
