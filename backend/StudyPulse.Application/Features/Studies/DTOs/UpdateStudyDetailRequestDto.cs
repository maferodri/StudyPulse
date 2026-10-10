namespace StudyPulse.Application.Features.Studies.DTOs;

public class UpdateStudyDetailRequestDto
{
    public string? Descripcion { get; set; } = string.Empty;
    public DateTimeOffset? FechaEntrega { get; set; }
    public int? StudyId { get; set; }
    public int? StudyStatusId { get; set; }
}