namespace StudyPulse.Application.Features.Studies.DTOs;

public class StudyDetailResponseDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset FechaEntrega { get; set; }
    public string StudyNombre { get; set; } = string.Empty; 
    public string StudyStatusNombre { get; set; } = string.Empty;
}