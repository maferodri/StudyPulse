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
    private readonly IValidator<CreateStudyRequestDto> _validator;
    
    public StudyController(IStudyRepository repository, IValidator<CreateStudyRequestDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

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
    
    [HttpPost]
    public async Task<IActionResult> CrearStudy([FromBody] CreateStudyRequestDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
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
}