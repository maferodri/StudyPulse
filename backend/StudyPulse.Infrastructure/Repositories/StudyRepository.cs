using Microsoft.EntityFrameworkCore;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Infrastructure.Data;

namespace StudyPulse.Infrastructure.Repositories;

//Se implementa la interfaz 
public class StudyRepository: IStudyRepository {
    private readonly StudyPulseDbContext _context;
    
    // El constructor recibe el contexto de EF Core
    public StudyRepository(StudyPulseDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<StudyGlobal>> GetAllAsync() {
        return await _context.StudyGlobales.ToListAsync(); // Consulta real a BD (Magia SQL)
    }

    public async Task<StudyGlobal> AddAsync(StudyGlobal study){
        await _context.StudyGlobales.AddAsync(study);
        
        await _context.SaveChangesAsync();
        return study;
    }
}

