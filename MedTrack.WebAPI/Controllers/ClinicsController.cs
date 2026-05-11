using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicService _service;
        private readonly ILogger<ClinicsController> _logger;

        public ClinicsController(IClinicService service, ILogger<ClinicsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicDto>>> GetAll()
        {
            try
            {
                var clinics = await _service.GetAllAsync();
                return Ok(clinics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all clinics");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicDto>> GetById(Guid id)
        {
            try
            {
                var clinic = await _service.GetByIdAsync(id);
                if (clinic == null) return NotFound();
                return Ok(clinic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting clinic {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClinicDto>> Create([FromBody] ClinicCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var clinic = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = clinic.Id }, clinic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating clinic");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClinicDto>> Update(Guid id, [FromBody] ClinicUpdateDto dto)
        {
            try
            {
                var clinic = await _service.UpdateAsync(id, dto);
                return Ok(clinic);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating clinic {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting clinic {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
