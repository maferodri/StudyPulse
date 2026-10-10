using Microsoft.EntityFrameworkCore;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Infrastructure.Data;

namespace StudyPulse.Infrastructure.Repositories;

public class StudyDetailRepository: IStudyDetailRepository
{
    private readonly StudyPulseDbContext _context;

    public StudyDetailRepository(StudyPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudyDetail>> GetAllAsync()
    {
        return await _context.Set<StudyDetail>()
            .Include( sd => sd.Study)
            .Include(sd => sd.StudyStatus)
            .ToListAsync();
    }

    public async Task<StudyDetail?> GetByIdAsync(int id)
    {
        return await _context.StudyDetails
            .Include(sd => sd.Study)
            .Include(sd => sd.StudyStatus)
            .FirstOrDefaultAsync(sd => sd.Id == id);
    }

    public async Task<StudyDetail> AddAsync(StudyDetail studyDetail)
    {
        studyDetail.FechaCreacion = studyDetail.FechaCreacion.ToUniversalTime();
        studyDetail.FechaEntrega = studyDetail.FechaEntrega.ToUniversalTime();
        
        await _context.StudyDetails.AddAsync(studyDetail);
        await _context.SaveChangesAsync();
        
        await _context.Entry(studyDetail).Reference(sd => sd.Study).LoadAsync();
        await _context.Entry(studyDetail).Reference(sd => sd.StudyStatus).LoadAsync();
        
        return studyDetail;
    }

    public async Task<StudyDetail> UpdateAsync(StudyDetail studyDetail)
    {
        _context.StudyDetails.Update(studyDetail);
        await _context.SaveChangesAsync();
        return studyDetail;
    }

    public Task DeleteAsync(StudyDetail studyDetail)
    {
        _context.StudyDetails.Remove(studyDetail);
        return _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByStudyIdAsync(int studyId)
    {
        return await _context.StudyDetails.AnyAsync(d => d.StudyId == studyId);
    }

    public async Task<bool> ExistsByStudyStatusIdAsync(int studyStatusId)
    {
        return await _context.StudyDetails.AnyAsync(d => d.StudyStatusId == studyStatusId);
    }
}