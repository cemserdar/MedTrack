using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalNotesController : ControllerBase
    {
        private readonly IMedicalNoteService _service;
        private readonly ILogger<MedicalNotesController> _logger;

        public MedicalNotesController(IMedicalNoteService service, ILogger<MedicalNotesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalNoteDto>>> GetAll()
        {
            try
            {
                var notes = await _service.GetAllAsync();
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all medical notes");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalNoteDto>> GetById(Guid id)
        {
            try
            {
                var note = await _service.GetByIdAsync(id);
                if (note == null) return NotFound();
                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting medical note {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalNoteDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var notes = await _service.GetByPatientAsync(patientId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting medical notes for patient {PatientId}", patientId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<MedicalNoteDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var notes = await _service.GetByDoctorAsync(doctorId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting medical notes for doctor {DoctorId}", doctorId);
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicalNoteDto>> Create([FromBody] MedicalNoteCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var note = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medical note");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MedicalNoteDto>> Update(Guid id, [FromBody] MedicalNoteUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var note = await _service.UpdateAsync(id, dto);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Medical note with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medical note {Id}", id);
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
                    return NotFound($"Medical note with ID {id} not found");

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Medical note with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medical note {Id}", id);
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
