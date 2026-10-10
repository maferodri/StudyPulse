using Microsoft.EntityFrameworkCore;
using StudyPulse.Domain.Entities;

namespace StudyPulse.Infrastructure.Data;

public class StudyPulseDbContext: DbContext
{
    public StudyPulseDbContext(DbContextOptions<StudyPulseDbContext> options) : base(options) { }
    
    public DbSet<StudyGlobal> StudyGlobales { get; set; } 
    public DbSet<StudyDetail> StudyDetails { get; set; }
    
    public DbSet<StudyStatus> StudyStatuses { get; set; }
}


