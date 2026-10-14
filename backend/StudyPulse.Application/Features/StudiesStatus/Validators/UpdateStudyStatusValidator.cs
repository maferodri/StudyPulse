using FluentValidation;
using StudyPulse.Application.Features.Studies.DTOs;

namespace StudyPulse.Application.Features.Studies.Validators;

public class UpdateStudyStatusValidator: AbstractValidator<UpdateStudyStatusRequestDto>
{
    public UpdateStudyStatusValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El estado no puede estar vacio")
            .MinimumLength(3).WithMessage("El estado no puede ser menor de 3 caracteres")
            .MaximumLength(50).WithMessage("El estado no puede ser mayor de 50 caracteres");
    }
}