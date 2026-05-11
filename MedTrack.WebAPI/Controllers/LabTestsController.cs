using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabTestsController : ControllerBase
    {
        private readonly ILabTestService _service;
        private readonly ILogger<LabTestsController> _logger;

        public LabTestsController(ILabTestService service, ILogger<LabTestsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabTestDto>>> GetAll()
        {
            try
            {
                var tests = await _service.GetAllAsync();
                return Ok(tests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all lab tests");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LabTestDto>> GetById(Guid id)
        {
            try
            {
                var test = await _service.GetByIdAsync(id);
                if (test == null) return NotFound();
                return Ok(test);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lab test {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<LabTestDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var tests = await _service.GetByPatientAsync(patientId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lab tests for patient {PatientId}", patientId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<LabTestDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var tests = await _service.GetByDoctorAsync(doctorId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lab tests for doctor {DoctorId}", doctorId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<LabTestDto>>> GetPending()
        {
            try
            {
                var tests = await _service.GetPendingAsync();
                return Ok(tests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending lab tests");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LabTestDto>> Create([FromBody] LabTestCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var test = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = test.Id }, test);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab test");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LabTestDto>> Update(Guid id, [FromBody] LabTestUpdateDto dto)
        {
            try
            {
                var test = await _service.UpdateAsync(id, dto);
                return Ok(test);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lab test {Id}", id);
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
                _logger.LogError(ex, "Error deleting lab test {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
