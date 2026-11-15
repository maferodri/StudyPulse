namespace StudyPulse.Domain.Entities;

public class ActivityType {
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = "#6B7280";
    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Activity> Actividades { get; set; } = new List<Activity>();
}
