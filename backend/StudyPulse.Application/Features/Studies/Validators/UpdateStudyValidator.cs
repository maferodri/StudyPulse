using FluentValidation;
using StudyPulse.Application.Features.Studies.DTOs;

namespace StudyPulse.Application.Features.Studies.Validators;

public class UpdateStudyValidator: AbstractValidator<UpdateStudyRequestDto>
{
    public UpdateStudyValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la actividad no puede estar vacio.")
            .MinimumLength(3).WithMessage("El nombre de la actividad debe tener al menos 3 caracteres.")
            .MaximumLength(100).WithMessage("El nombre de la actividad es demasiado largo.");
    }
}