using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Studies.DTOs;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudyStatusController: Controller
{
    private readonly IStudyStatusRepository _repository;
    private readonly IStudyDetailRepository _detailRepository;
    private readonly IValidator<CreateStudyStatusRequestDto> _createValidator;
    private readonly IValidator<UpdateStudyStatusRequestDto> _updateValidator;

    public StudyStatusController(
        IStudyStatusRepository repository,
        IStudyDetailRepository detailRepository,
        IValidator<CreateStudyStatusRequestDto> createValidator,
        IValidator<UpdateStudyStatusRequestDto> updateValidator)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudiesStatus()
    {
        var studiesStatus = await _repository.GetAllAsync();

        var response = studiesStatus.Select(s => new StudyStatusResponseDto
        {
            Id = s.Id,
            Nombre = s.Nombre
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudiesStatusById(int id)
    {
        var studiesStatus = await _repository.GetByIdAsync(id);

        if (studiesStatus == null)
        {
            return NotFound(new {mensaje = $"No se encontró el estado con el ID {id}"});
        }

        var response = new StudyStatusResponseDto
        {
            Id = studiesStatus.Id,
            Nombre = studiesStatus.Nombre
        };
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudyStatus([FromBody] CreateStudyStatusRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var nuevoStudyStatus = new StudyStatus
        {
            Nombre = request.Nombre
        };

        var studyStatusCreado = await _repository.AddAsync(nuevoStudyStatus);

        var response = new StudyStatusResponseDto
        {
            Id = studyStatusCreado.Id,
            Nombre = studyStatusCreado.Nombre
        };
        
        return CreatedAtAction(nameof(GetStudiesStatusById), new { id = studyStatusCreado.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudyStatus(int id, [FromBody] UpdateStudyStatusRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var studiesStatus = await _repository.GetByIdAsync(id);
        if (studiesStatus == null)
        {
            return NotFound(new { mensaje = $"No se encontro el estado con el ID {id}" });
        }
        
        if (await _repository.ExistsByNombreAsync(request.Nombre, excludeId: id))
        {
            return Conflict(new { mensaje = "Ya existe un estado con ese nombre." });
        }

        if (studiesStatus.Nombre == request.Nombre)
        {
            return BadRequest(new {mensaje = "El nuevo nombre del estado no puede ser idéntico al nombre actual"});
        }
        
        studiesStatus.Nombre = request.Nombre;
        
        await _repository.UpdateAsync(studiesStatus);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudyStatus(int id)
    {
        var studiesStatus = await _repository.GetByIdAsync(id);
        if (studiesStatus == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estado con el ID {id}" });
        }

        if (await _detailRepository.ExistsByStudyStatusIdAsync(id))
        {
            return Conflict(new { mensaje = "No se puede eliminar el estado porque tiene tareas asociadas." });
        }
        
        await _repository.DeleteAsync(studiesStatus);
        return NoContent();
    }
}