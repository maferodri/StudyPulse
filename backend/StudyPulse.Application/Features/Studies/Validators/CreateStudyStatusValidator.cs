using FluentValidation;
using StudyPulse.Application.Features.Studies.DTOs;
using StudyPulse.Application.Interfaces;

namespace StudyPulse.Application.Features.Studies.Validators;

public class CreateStudyStatusValidator: AbstractValidator<CreateStudyStatusRequestDto>
{
    public CreateStudyStatusValidator(IStudyStatusRepository studyStatusRepository)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El estado no puede estar vacio")
            .MinimumLength(3).WithMessage("El estado no puede ser menor de 3 caracteres")
            .MaximumLength(50).WithMessage("El estado no puede ser mayor de 50 caracteres")
            .MustAsync(async (nombre, ct) => !await studyStatusRepository.ExistsByNombreAsync(nombre))
            .WithMessage("Ya existe un estado con ese nombre.");
    }
}