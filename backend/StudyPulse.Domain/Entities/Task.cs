namespace StudyPulse.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset FechaEntrega { get; set; }
    
    //Referencia a la tabla Activity
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!; //Null-Forgiving
    
    //Referencia a la tabla StudyStatus
    public int StudyStatusId { get; set; }
    public StudyStatus StudyStatus { get; set; } = null!;
}