using StudyPulse.Domain.Entities;

using Task = System.Threading.Tasks.Task;
using TaskStatus = StudyPulse.Domain.Entities.TaskStatus;

namespace StudyPulse.Application.Interfaces;

public interface ITaskStatusRepository
{
    Task<IEnumerable<TaskStatus>> GetAllAsync();
    Task<TaskStatus?> GetByIdAsync(int id);
    Task<TaskStatus> AddAsync(TaskStatus study);
    Task<TaskStatus> UpdateAsync(TaskStatus study);
    Task DeleteAsync(TaskStatus study);
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
}
