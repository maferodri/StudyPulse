using Microsoft.EntityFrameworkCore;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace StudyPulse.Infrastructure.Repositories;

//Se implementa la interfaz 
public class ActivityRepository: IActivityRepository {
    private readonly StudyPulseDbContext _context;
    
    // El constructor recibe el contexto de EF Core
    public ActivityRepository(StudyPulseDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync() {
        return await _context.Activities.ToListAsync(); // Consulta real a BD (Magia SQL)
    }

    public async Task<Activity> AddAsync(Activity activity){
        await _context.Activities.AddAsync(activity);
        
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<Activity?> GetByIdAsync(int id)
    {
        return await _context.Activities.FindAsync(id);
    }

    public async Task<Activity> UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public Task DeleteAsync(Activity activity)
    {
        _context.Activities.Remove(activity);
        return _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        var normalizado = nombre.Trim().ToLower();
        return await _context.Set<Activity>().
            AnyAsync(g => g.Nombre.ToLower() == normalizado && g.Id != excludeId);
    }
}

