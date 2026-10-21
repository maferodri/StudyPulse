using Microsoft.EntityFrameworkCore;
using StudyPulse.Domain.Entities;
using Task = StudyPulse.Domain.Entities.Task;

namespace StudyPulse.Infrastructure.Data;

public class StudyPulseDbContext: DbContext
{
    public StudyPulseDbContext(DbContextOptions<StudyPulseDbContext> options) : base(options) { }
    
    public DbSet<Activity> Activities { get; set; } 
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<StudyStatus> StudyStatuses { get; set; }
}


