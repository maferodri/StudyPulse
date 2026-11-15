using StudyPulse.Domain.Entities;
using Task = StudyPulse.Domain.Entities.Task;

namespace StudyPulse.Application.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<Task>> GetAllAsync();
    Task<Task?> GetByIdAsync(int id);
    Task<Task> AddAsync(Task task);
    Task <Task> UpdateAsync(Task task);
    System.Threading.Tasks.Task DeleteAsync(Task task);
    
    Task<bool> ExistsByTaskIdAsync(int activityId);
    Task<bool> ExistsByTaskStatusIdAsync(int studyStatusId);
}