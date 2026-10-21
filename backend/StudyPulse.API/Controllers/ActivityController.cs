using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Activities.DTOs;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityController: ControllerBase {
    private readonly IActivityRepository _repository;
    private readonly ITaskRepository _detailRepository;
    private readonly IValidator<CreateActivityRequestDto> _createValidator;
    private readonly IValidator<UpdateActivityRequestDto> _updateValidator;
    
    public ActivityController(
        IActivityRepository repository, 
        ITaskRepository detailRepository,
        IValidator<CreateActivityRequestDto> createValidator,
        IValidator<UpdateActivityRequestDto> updateValidator)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _createValidator = createValidator;
        _updateValidator =  updateValidator;
    }

    //Lista de todos los elementos 
    [HttpGet]
    public async Task<IActionResult> GetActivities()
    {
        var activities = await _repository.GetAllAsync();

        // 1. Mapeas a DTO
        var response = activities.Select(s => new ActivityResponseDto
        {
            Id = s.Id,
            Nombre = s.Nombre
        });
        
        // CORRECCIÓN 1: Retornas 'response', no 'studies'
        return Ok(response); 
    }
    
    //Obtener elemento por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivitiesById(int id)
    {
        var activities = await _repository.GetByIdAsync(id);

        if (activities == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estudio con el ID {id}" });
        }

        var response = new ActivityResponseDto
        {
            Id = activities.Id,
            Nombre = activities.Nombre
        };
        
        return Ok(response);
    }
    
    //Agregar un elemento nuevo
    [HttpPost]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newActivity = new Activity
        {
            Nombre = request.Nombre
        };
        
        var activityCreated = await _repository.AddAsync(newActivity);

        // 2. Mapeas a DTO
        var response = new ActivityResponseDto
        {
            Id = activityCreated.Id,
            Nombre = activityCreated.Nombre
        };
        return CreatedAtAction(nameof(GetActivities), new { id = activityCreated.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(int id, [FromBody] UpdateActivityRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var activities = await _repository.GetByIdAsync(id);
        if (activities == null)
        {
            return NotFound(new { mensaje = $"No se encontró la actividad con el ID {id}" });
        }
        
        if (await _repository.ExistsByNombreAsync(request.Nombre, excludeId: id))
        {
            return Conflict(new { mensaje = "Ya existe una actividad con ese nombre." });
        }

        if (activities.Nombre == request.Nombre)
        {
            return BadRequest(new {mensaje = "El nuevo nombre no puede ser idéntico al nombre actual"});
        }
        
        activities.Nombre = request.Nombre;
        
        await _repository.UpdateAsync(activities);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var activities = await _repository.GetByIdAsync(id);
        if (activities == null)
        {
            return NotFound(new { mensaje = $"No se encontró la actividad con el ID {id}" });
        }
        
        if (await _detailRepository.ExistsByStudyIdAsync(id))
        {
            return Conflict(new { mensaje = "No se puede eliminar la actividad porque tiene tareas asociadas." });
        }
        
        await _repository.DeleteAsync(activities);
        return NoContent();
    }
}