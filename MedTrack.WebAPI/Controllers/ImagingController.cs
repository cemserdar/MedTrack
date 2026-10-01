using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagingController : ControllerBase
    {
        private readonly IImagingService _service;
        private readonly ILogger<ImagingController> _logger;

        public ImagingController(IImagingService service, ILogger<ImagingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagingResultDto>>> GetAll()
        {
            try
            {
                var results = await _service.GetAllAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all imaging results");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImagingResultDto>> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting imaging result {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<ImagingResultDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var results = await _service.GetByPatientAsync(patientId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting imaging results for patient {PatientId}", patientId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<ImagingResultDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var results = await _service.GetByDoctorAsync(doctorId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting imaging results for doctor {DoctorId}", doctorId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ImagingResultDto>>> GetPending()
        {
            try
            {
                var results = await _service.GetPendingAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending imaging results");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ImagingResultDto>> Create([FromBody] ImagingResultCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating imaging result");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ImagingResultDto>> Update(Guid id, [FromBody] ImagingResultUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Imaging result with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating imaging result {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound($"Imaging result with ID {id} not found");

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Imaging result with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting imaging result {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
