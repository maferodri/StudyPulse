using Microsoft.EntityFrameworkCore;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Interfaces;
using StudyPulse.Infrastructure.Data;

namespace StudyPulse.Infrastructure.Repositories;

public class StudyStatusRepository: IStudyStatusRepository
{
    private readonly StudyPulseDbContext _context;

    public StudyStatusRepository(StudyPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudyStatus>> GetAllAsync()
    {
        return await _context.StudyStatuses.ToListAsync();
    }

    public async Task<StudyStatus?> GetByIdAsync(int id)
    {
        return await _context.StudyStatuses.FindAsync(id);
    }

    public async Task<StudyStatus> AddAsync(StudyStatus studyStatus)
    {
        await _context.StudyStatuses.AddAsync(studyStatus);
        await _context.SaveChangesAsync();
        return studyStatus;
    }

    public async Task<StudyStatus> UpdateAsync(StudyStatus studyStatus)
    {
        _context.StudyStatuses.Update(studyStatus);
        await _context.SaveChangesAsync();
        return studyStatus;
    }

    public Task DeleteAsync(StudyStatus studyStatus)
    {
        _context.StudyStatuses.Remove(studyStatus);
        return _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        var normalizado = nombre.Trim().ToLower();
        return await _context.Set<StudyStatus>()
            .AnyAsync(s => s.Nombre.ToLower() == normalizado
                           && (excludeId == null || s.Id != excludeId));
    }
}

