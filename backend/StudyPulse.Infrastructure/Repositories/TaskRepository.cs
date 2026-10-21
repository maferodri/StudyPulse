using Microsoft.EntityFrameworkCore;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Infrastructure.Data;
using Task = StudyPulse.Domain.Entities.Task;

namespace StudyPulse.Infrastructure.Repositories;

public class TaskRepository: ITaskRepository
{
    private readonly StudyPulseDbContext _context;

    public TaskRepository(StudyPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
    {
        return await _context.Set<Task>()
            .Include( sd => sd.Activity)
            .Include(sd => sd.StudyStatus)
            .ToListAsync();
    }

    public async Task<Task?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(sd => sd.Activity)
            .Include(sd => sd.StudyStatus)
            .FirstOrDefaultAsync(sd => sd.Id == id);
    }

    public async Task<Task> AddAsync(Task task)
    {
        task.FechaCreacion = task.FechaCreacion.ToUniversalTime();
        task.FechaEntrega = task.FechaEntrega.ToUniversalTime();
        
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        await _context.Entry(task).Reference(sd => sd.Activity).LoadAsync();
        await _context.Entry(task).Reference(sd => sd.StudyStatus).LoadAsync();
        
        return task;
    }

    public async Task<Task> UpdateAsync(Task task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public System.Threading.Tasks.Task DeleteAsync(Task task)
    {
        _context.Tasks.Remove(task);
        return _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByStudyIdAsync(int activityId)
    {
        return await _context.Tasks.AnyAsync(d => d.ActivityId == activityId);
    }

    public async Task<bool> ExistsByStudyStatusIdAsync(int studyStatusId)
    {
        return await _context.Tasks.AnyAsync(d => d.StudyStatusId == studyStatusId);
    }
}