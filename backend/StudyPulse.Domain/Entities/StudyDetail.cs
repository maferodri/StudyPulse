namespace StudyPulse.Domain.Entities;

public class StudyDetail
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset FechaEntrega { get; set; }
    
    //Referencia a la tabla StudyGlobal
    public int StudyId { get; set; }
    public StudyGlobal Study { get; set; } = null!; //Null-Forgiving
    
    //Referencia a la tabla StudyStatus
    public int StudyStatusId { get; set; }
    public StudyStatus StudyStatus { get; set; } = null!;
}