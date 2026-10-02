using StudyPulse.Domain.Entities;

namespace StudyPulse.Application.Interfaces;

public interface IStudyRepository
{
    Task<IEnumerable<StudyGlobal>> GetAllAsync();
    Task<StudyGlobal> AddAsync(StudyGlobal study);
}