namespace StudyPulse.Application.Features.Tasks.DTOs;

public class UpdateTaskRequestDto
{
    public string? Descripcion { get; set; } = string.Empty;
    public DateTimeOffset? FechaEntrega { get; set; }
    public int? ActivityId { get; set; }
    public int? StudyStatusId { get; set; }
}