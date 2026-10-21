using FluentValidation;
using StudyPulse.Application.Features.Tasks.DTOs;
using StudyPulse.Application.Interfaces;

namespace StudyPulse.Application.Features.Tasks.Validators;

public class UpdateTaskValidator: AbstractValidator<UpdateTaskRequestDto>
{
    public  UpdateTaskValidator(IActivityRepository activityRepository, IStudyStatusRepository studyStatusRepository)
    {
        RuleFor(s => s.Descripcion)
            .Must(d => !string.IsNullOrWhiteSpace(d)).WithMessage("La descripción no puede estar vacía")
            .MinimumLength(3).WithMessage("La decripción debe tener un minimo 3 caracteres")
            .MaximumLength(100).WithMessage("La descripcion debe tener un maximo 100 caracteres")
            .When(s => s.Descripcion is not null);

        RuleFor(s => s.FechaEntrega)
            .Must(f => f!.Value > DateTimeOffset.UtcNow)
            .WithMessage("La fecha de entrega debe ser en el futuro")
            .When(s => s.FechaEntrega.HasValue);

        RuleFor(s => s.ActivityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una actividad válida.")
            .MustAsync(async (id, ct) => await activityRepository.GetByIdAsync(id!.Value) != null)
            .WithMessage("No existe una actividad con ese ID.")
            .When(s => s.ActivityId.HasValue);

        RuleFor(s => s.StudyStatusId)
            .GreaterThan(0).WithMessage("Debe seleccionar un estado válido.")
            .MustAsync(async (id, ct) => await studyStatusRepository.GetByIdAsync(id!.Value) != null)
            .WithMessage("No existe un estado con ese ID.")
            .When(s => s.StudyStatusId.HasValue);
    }
}