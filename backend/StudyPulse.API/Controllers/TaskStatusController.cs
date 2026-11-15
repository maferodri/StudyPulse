using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Studies.DTOs;
using TaskStatus = StudyPulse.Domain.Entities.TaskStatus;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TaskStatusController: Controller
{
    private readonly ITaskStatusRepository _repository;
    private readonly ITaskRepository _detailRepository;
    private readonly IValidator<CreateTaskStatusRequestDto> _createValidator;
    private readonly IValidator<UpdateTaskStatusRequestDto> _updateValidator;

    public TaskStatusController(
        ITaskStatusRepository repository,
        ITaskRepository detailRepository,
        IValidator<CreateTaskStatusRequestDto> createValidator,
        IValidator<UpdateTaskStatusRequestDto> updateValidator)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasksStatus()
    {
        var tasksStatus = await _repository.GetAllAsync();

        var response = tasksStatus.Select(s => new TaskStatusResponseDto
        {
            Id = s.Id,
            Nombre = s.Nombre
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTasksStatusById(int id)
    {
        var tasksStatus = await _repository.GetByIdAsync(id);

        if (tasksStatus == null)
        {
            return NotFound(new {mensaje = $"No se encontró el estado con el ID {id}"});
        }

        var response = new TaskStatusResponseDto
        {
            Id = tasksStatus.Id,
            Nombre = tasksStatus.Nombre
        };
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskStatus([FromBody] CreateTaskStatusRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var nuevoTaskStatus = new TaskStatus
        {
            Nombre = request.Nombre
        };

        var taskStatusCreado = await _repository.AddAsync(nuevoTaskStatus);

        var response = new TaskStatusResponseDto
        {
            Id = taskStatusCreado.Id,
            Nombre = taskStatusCreado.Nombre
        };
        
        return CreatedAtAction(nameof(GetTasksStatusById), new { id = taskStatusCreado.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var tasksStatus = await _repository.GetByIdAsync(id);
        if (tasksStatus == null)
        {
            return NotFound(new { mensaje = $"No se encontro el estado con el ID {id}" });
        }
        
        if (await _repository.ExistsByNombreAsync(request.Nombre, excludeId: id))
        {
            return Conflict(new { mensaje = "Ya existe un estado con ese nombre." });
        }

        if (tasksStatus.Nombre == request.Nombre)
        {
            return BadRequest(new {mensaje = "El nuevo nombre del estado no puede ser idéntico al nombre actual"});
        }
        
        tasksStatus.Nombre = request.Nombre;
        
        await _repository.UpdateAsync(tasksStatus);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskStatus(int id)
    {
        var tasksStatus = await _repository.GetByIdAsync(id);
        if (tasksStatus == null)
        {
            return NotFound(new { mensaje = $"No se encontró el estado con el ID {id}" });
        }

        if (await _detailRepository.ExistsByTaskStatusIdAsync(id))
        {
            return Conflict(new { mensaje = "No se puede eliminar el estado porque tiene tareas asociadas." });
        }
        
        await _repository.DeleteAsync(tasksStatus);
        return NoContent();
    }
}