using FluentValidation;
using StudyPulse.Application.Features.Activities.DTOs;
using StudyPulse.Application.Interfaces;

namespace StudyPulse.Application.Features.Activities.Validators;

public class CreateActivityValidator: AbstractValidator<CreateActivityRequestDto>
{
    public CreateActivityValidator(IActivityRepository activityRepository)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la actividad no puede estar vacio.")
            .MinimumLength(3).WithMessage("El nombre de la actividad debe tener al menos 3 caracteres.")
            .MaximumLength(100).WithMessage("El nombre de la actividad es demasiado largo.")
            .MustAsync(async (nombre, ct) => !await activityRepository.ExistsByNombreAsync(nombre))
            .WithMessage("Ya existe una actividad con ese nombre.");
    }
}