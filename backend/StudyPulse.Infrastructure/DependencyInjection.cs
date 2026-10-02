using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyPulse.Application.Interfaces;
using StudyPulse.Infrastructure.Repositories;
using StudyPulse.Infrastructure.Data;


namespace StudyPulse.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        //Configuramos el acceso a postgres
        services.AddDbContext<StudyPulseDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        //La Inyección
        services.AddScoped<IStudyRepository, StudyRepository>();
        return services;
    }
    
}