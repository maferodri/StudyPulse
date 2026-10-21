namespace StudyPulse.Application.Features.Tasks.DTOs;

public class CreateTaskRequestDto
{
    public string Descripcion { get; set; } = string.Empty;
    public DateTimeOffset FechaEntrega { get; set; }
    public int StudyId { get; set; }
    public int StudyStatusId { get; set; }
}
