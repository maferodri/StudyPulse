using FluentValidation;
using StudyPulse.Application.Features.Studies.DTOs;
using StudyPulse.Application.Interfaces;

namespace StudyPulse.Application.Features.Studies.Validators;

public class CreateStudyDetailValidator: AbstractValidator<CreateStudyDetailRequestDto>
{
    public CreateStudyDetailValidator(IStudyStatusRepository studyStatusRepository, IStudyRepository studyRepository)
    {
        RuleFor(s => s.Descripcion)
            .NotEmpty().WithMessage("Descripcion no puede estar vacio")
            .MinimumLength(3).WithMessage("Descripcion minimo 3 caracteres")
            .MaximumLength(100).WithMessage("Descripcion maximo 100 caracteres");
        
        RuleFor(s => s.FechaEntrega)
            .NotEmpty().WithMessage("La fecha no puede estar vacia")
            .GreaterThan(_ => DateTimeOffset.UtcNow).WithMessage("La fecha de entrega debe ser en el futuro");
        
        RuleFor(s => s.StudyId)
            .GreaterThan(0).WithMessage("Debe seleccionar una actividad válido.")
            .MustAsync(async (id, ct) => await studyRepository.GetByIdAsync(id) != null)
            .WithMessage("No existe una actividad con ese ID.");
        
        RuleFor(s => s.StudyStatusId)
            .GreaterThan(0).WithMessage("Debe seleccionar un estado válido.")
            .MustAsync(async (id, ct) => await studyStatusRepository.GetByIdAsync(id) != null)
            .WithMessage("No existe un estado con ese ID.");
    }
}