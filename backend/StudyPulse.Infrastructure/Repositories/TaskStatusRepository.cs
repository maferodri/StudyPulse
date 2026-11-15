using Microsoft.EntityFrameworkCore;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Interfaces;
using StudyPulse.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;
using TaskStatus = StudyPulse.Domain.Entities.TaskStatus;

namespace StudyPulse.Infrastructure.Repositories;

public class TaskStatusRepository: ITaskStatusRepository
{
    private readonly StudyPulseDbContext _context;

    public TaskStatusRepository(StudyPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskStatus>> GetAllAsync()
    {
        return await _context.TasksStatus.ToListAsync();
    }

    public async Task<TaskStatus?> GetByIdAsync(int id)
    {
        return await _context.TasksStatus.FindAsync(id);
    }

    public async Task<TaskStatus> AddAsync(TaskStatus taskStatus)
    {
        await _context.TasksStatus.AddAsync(taskStatus);
        await _context.SaveChangesAsync();
        return taskStatus;
    }

    public async Task<TaskStatus> UpdateAsync(TaskStatus taskStatus)
    {
        _context.TasksStatus.Update(taskStatus);
        await _context.SaveChangesAsync();
        return taskStatus;
    }

    public Task DeleteAsync(TaskStatus taskStatus)
    {
        _context.TasksStatus.Remove(taskStatus);
        return _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        var normalizado = nombre.Trim().ToLower();
        return await _context.Set<TaskStatus>()
            .AnyAsync(s => s.Nombre.ToLower() == normalizado
                           && (excludeId == null || s.Id != excludeId));
    }
}
