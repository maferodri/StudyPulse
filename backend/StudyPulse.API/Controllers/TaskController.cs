using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Tasks.DTOs;
using Task = StudyPulse.Domain.Entities.Task;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController: ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly IValidator<CreateTaskRequestDto> _createValidator;
    private readonly IValidator<UpdateTaskRequestDto> _updateValidator;
    
    public TaskController(ITaskRepository repository, IValidator<CreateTaskRequestDto> createValidator, IValidator<UpdateTaskRequestDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskById()
    {
        var tasks = await _repository.GetAllAsync();

        var response = tasks.Select(h => new TaskResponseDto
        {
            Id = h.Id,
            Descripcion = h.Descripcion,
            FechaCreacion = h.FechaCreacion,
            FechaEntrega = h.FechaEntrega,
            StudyNombre = h.Activity.Nombre,
            StudyStatusNombre =  h.StudyStatus.Nombre
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var tasks = await _repository.GetByIdAsync(id);

        if (tasks == null)
        {
            return NotFound(new{mensaje = $"No se encontro la tarea con el ID {id}" });
        }
        
        var response = new TaskResponseDto
        {
            Id = tasks.Id,
            Descripcion = tasks.Descripcion,
            FechaCreacion = tasks.FechaCreacion,
            FechaEntrega = tasks.FechaEntrega,
            StudyNombre = tasks.Activity.Nombre,
            StudyStatusNombre = tasks.StudyStatus.Nombre
        };
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newTask = new Task
        {
            Descripcion = request.Descripcion,
            FechaEntrega = request.FechaEntrega,
            ActivityId = request.StudyId,
            StudyStatusId =  request.StudyStatusId
        };

        var taskCreated = await _repository.AddAsync(newTask);

        var response = new TaskResponseDto
        {
            Id = taskCreated.Id,
            Descripcion = taskCreated.Descripcion,
            FechaCreacion = taskCreated.FechaCreacion,
            FechaEntrega = request.FechaEntrega.ToUniversalTime(),
            StudyNombre = taskCreated.Activity.Nombre,
            StudyStatusNombre = taskCreated.StudyStatus.Nombre
        };
        
        return CreatedAtAction(nameof(GetTaskById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var tasks = await _repository.GetByIdAsync(id);
        if (tasks == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con el ID {id}" });
        }

        var huboCambios = false;

        if (request.Descripcion is not null && request.Descripcion != tasks.Descripcion)
        {
            tasks.Descripcion = request.Descripcion;
            huboCambios = true;
        }

        if (request.FechaEntrega.HasValue && request.FechaEntrega.Value.ToUniversalTime() != tasks.FechaEntrega)
        {
            tasks.FechaEntrega = request.FechaEntrega.Value;
            huboCambios = true;
        }

        if (request.ActivityId.HasValue && request.ActivityId.Value != tasks.ActivityId)
        {
            tasks.ActivityId = request.ActivityId.Value;
            huboCambios = true;
        }

        if (!huboCambios)
        {
            return BadRequest(new { mensaje = "No se detectaron cambios respecto a los valores actuales" });
        }

        await _repository.UpdateAsync(tasks);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var tasks = await _repository.GetByIdAsync(id);
        if (tasks == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con el ID {id}" });
        }
        
        await _repository.DeleteAsync(tasks);
        return NoContent();
    }
}