using FluentValidation;
using StudyPulse.Application.Features.Activities.DTOs;

namespace StudyPulse.Application.Features.Activities.Validators;

public class UpdateActivityValidator: AbstractValidator<UpdateActivityRequestDto>
{
    public UpdateActivityValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la actividad no puede estar vacio.")
            .MinimumLength(3).WithMessage("El nombre de la actividad debe tener al menos 3 caracteres.")
            .MaximumLength(100).WithMessage("El nombre de la actividad es demasiado largo.");
    }
}