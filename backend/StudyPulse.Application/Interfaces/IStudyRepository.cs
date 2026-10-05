using StudyPulse.Domain.Entities;

namespace StudyPulse.Application.Interfaces;

public interface IStudyRepository
{
    Task<IEnumerable<StudyGlobal>> GetAllAsync();
    Task<StudyGlobal> AddAsync(StudyGlobal study);
    //EL ? permite que el método devuelva un valor nulo
    Task<StudyGlobal?> GetByIdAsync(int id);
    Task<StudyGlobal> UpdateAsync(StudyGlobal study);
    Task DeleteAsync(StudyGlobal study);
}