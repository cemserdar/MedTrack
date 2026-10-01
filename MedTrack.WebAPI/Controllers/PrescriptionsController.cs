using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly ILogger<PrescriptionsController> _logger;

        public PrescriptionsController(IPrescriptionService service, ILogger<PrescriptionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
        {
            try
            {
                var prescriptions = await _service.GetAllAsync();
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all prescriptions");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> GetById(Guid id)
        {
            try
            {
                var prescription = await _service.GetByIdAsync(id);
                if (prescription == null) return NotFound();
                return Ok(prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting prescription {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var prescriptions = await _service.GetByPatientAsync(patientId);
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting prescriptions for patient {PatientId}", patientId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var prescriptions = await _service.GetByDoctorAsync(doctorId);
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting prescriptions for doctor {DoctorId}", doctorId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> Create([FromBody] PrescriptionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var prescription = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = prescription.Id }, prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prescription");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PrescriptionDto>> Update(Guid id, [FromBody] PrescriptionUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var prescription = await _service.UpdateAsync(id, dto);
                return Ok(prescription);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Prescription with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prescription {Id}", id);
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
                    return NotFound($"Prescription with ID {id} not found");

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Prescription with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting prescription {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
