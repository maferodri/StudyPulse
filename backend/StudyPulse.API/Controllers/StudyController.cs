using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Studies.DTOs;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyController: ControllerBase {
    private readonly IStudyRepository _repository;
    private readonly IStudyDetailRepository _detailRepository;
    private readonly IValidator<CreateStudyRequestDto> _createValidator;
    private readonly IValidator<UpdateStudyRequestDto> _updateValidator;
    
    public StudyController(
        IStudyRepository repository, 
        IStudyDetailRepository detailRepository,
        IValidator<CreateStudyRequestDto> createValidator,
        IValidator<UpdateStudyRequestDto> updateValidator)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _createValidator = createValidator;
        _updateValidator =  updateValidator;
    }

    //Lista de todos los elementos 
    [HttpGet]
    public async Task<IActionResult> GetStudies()
    {
        var studies = await _repository.GetAllAsync();

        // 1. Mapeas a DTO
        var response = studies.Select(s => new StudyResponseDto
        {
            Id = s.Id,
            Nombre = s.Nombre
        });
        
        // CORRECCIÓN 1: Retornas 'response', no 'studies'
        return Ok(response); 
    }
    
    //Obtener elemento por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudiesById(int id)
    {
        var studies = await _repository.GetByIdAsync(id);

        if (studies == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estudio con el ID {id}" });
        }

        var response = new StudyResponseDto
        {
            Id = studies.Id,
            Nombre = studies.Nombre
        };
        
        return Ok(response);
    }
    
    //Agregar un elemento nuevo
    [HttpPost]
    public async Task<IActionResult> CrearStudy([FromBody] CreateStudyRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var nuevoStudy = new StudyGlobal
        {
            Nombre = request.Nombre
        };
        
        var studyCreado = await _repository.AddAsync(nuevoStudy);

        // 2. Mapeas a DTO
        var response = new StudyResponseDto
        {
            Id = studyCreado.Id,
            Nombre = studyCreado.Nombre
        };
        
        // CORRECCIÓN 2: Pasas 'response' como el objeto que se devolverá al cliente
        return CreatedAtAction(nameof(GetStudies), new { id = studyCreado.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudy(int id, [FromBody] UpdateStudyRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var studies = await _repository.GetByIdAsync(id);
        if (studies == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estudio con el ID {id}" });
        }
        
        if (await _repository.ExistsByNombreAsync(request.Nombre, excludeId: id))
        {
            return Conflict(new { mensaje = "Ya existe un estudio con ese nombre." });
        }

        if (studies.Nombre == request.Nombre)
        {
            return BadRequest(new {mensaje = "El nuevo nombre no puede ser idéntico al nombre actual"});
        }
        
        studies.Nombre = request.Nombre;
        
        await _repository.UpdateAsync(studies);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudy(int id)
    {
        var studies = await _repository.GetByIdAsync(id);
        if (studies == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estudio con el ID {id}" });
        }
        
        if (await _detailRepository.ExistsByStudyIdAsync(id))
        {
            return Conflict(new { mensaje = "No se puede eliminar el estudio porque tiene tareas asociadas." });
        }
        
        await _repository.DeleteAsync(studies);
        return NoContent();
    }
}