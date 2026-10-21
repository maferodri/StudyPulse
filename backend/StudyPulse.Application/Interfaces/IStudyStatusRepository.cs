using StudyPulse.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace StudyPulse.Application.Interfaces;

public interface IStudyStatusRepository
{
    Task<IEnumerable<StudyStatus>> GetAllAsync();
    Task<StudyStatus?> GetByIdAsync(int id);
    Task<StudyStatus> AddAsync(StudyStatus study);
    Task<StudyStatus> UpdateAsync(StudyStatus study);
    Task DeleteAsync(StudyStatus study);
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
}