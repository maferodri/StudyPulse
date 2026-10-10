using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StudyPulse.Application.Interfaces;
using StudyPulse.Domain.Entities;
using StudyPulse.Application.Features.Studies.DTOs;

namespace StudyPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyDetailController: ControllerBase
{
    private readonly IStudyDetailRepository _repository;
    private readonly IValidator<CreateStudyDetailRequestDto> _createValidator;
    private readonly IValidator<UpdateStudyDetailRequestDto> _updateValidator;
    
    public StudyDetailController(IStudyDetailRepository repository, IValidator<CreateStudyDetailRequestDto> createValidator, IValidator<UpdateStudyDetailRequestDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetHabitosDetail()
    {
        var habitoDetail = await _repository.GetAllAsync();

        var response = habitoDetail.Select(h => new StudyDetailResponseDto
        {
            Id = h.Id,
            Descripcion = h.Descripcion,
            FechaCreacion = h.FechaCreacion,
            FechaEntrega = h.FechaEntrega,
            StudyNombre = h.Study.Nombre,
            StudyStatusNombre =  h.StudyStatus.Nombre
        });
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHabitosDetail(int id)
    {
        var habitoDetail = await _repository.GetByIdAsync(id);

        if (habitoDetail == null)
        {
            return NotFound(new{mensaje = $"No se encontro el estudio con el ID {id}" });
        }
        
        var response = new StudyDetailResponseDto
        {
            Id = habitoDetail.Id,
            Descripcion = habitoDetail.Descripcion,
            FechaCreacion = habitoDetail.FechaCreacion,
            FechaEntrega = habitoDetail.FechaEntrega,
            StudyNombre = habitoDetail.Study.Nombre,
            StudyStatusNombre = habitoDetail.StudyStatus.Nombre
        };
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CrearHabitoDetail([FromBody] CreateStudyDetailRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var nuevoStudyDetail = new StudyDetail
        {
            Descripcion = request.Descripcion,
            FechaEntrega = request.FechaEntrega,
            StudyId = request.StudyId,
            StudyStatusId =  request.StudyStatusId
        };

        var studyDetailCreado = await _repository.AddAsync(nuevoStudyDetail);

        var response = new StudyDetailResponseDto
        {
            Id = studyDetailCreado.Id,
            Descripcion = studyDetailCreado.Descripcion,
            FechaCreacion = studyDetailCreado.FechaCreacion,
            FechaEntrega = request.FechaEntrega.ToUniversalTime(),
            StudyNombre = studyDetailCreado.Study.Nombre,
            StudyStatusNombre = studyDetailCreado.StudyStatus.Nombre
        };
        
        return CreatedAtAction(nameof(GetHabitosDetail), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudyDetail(int id, [FromBody] UpdateStudyDetailRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var studiesDetail = await _repository.GetByIdAsync(id);
        if (studiesDetail == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con el ID {id}" });
        }

        var huboCambios = false;

        if (request.Descripcion is not null && request.Descripcion != studiesDetail.Descripcion)
        {
            studiesDetail.Descripcion = request.Descripcion;
            huboCambios = true;
        }

        if (request.FechaEntrega.HasValue && request.FechaEntrega.Value.ToUniversalTime() != studiesDetail.FechaEntrega)
        {
            studiesDetail.FechaEntrega = request.FechaEntrega.Value;
            huboCambios = true;
        }

        if (request.StudyId.HasValue && request.StudyId.Value != studiesDetail.StudyId)
        {
            studiesDetail.StudyId = request.StudyId.Value;
            huboCambios = true;
        }

        if (!huboCambios)
        {
            return BadRequest(new { mensaje = "No se detectaron cambios respecto a los valores actuales" });
        }

        await _repository.UpdateAsync(studiesDetail);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudyDetail(int id)
    {
        var studiesDetails = await _repository.GetByIdAsync(id);
        if (studiesDetails == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con el ID {id}" });
        }
        
        await _repository.DeleteAsync(studiesDetails);
        return NoContent();
    }
}