using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IPatientService service, ILogger<PatientsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            try
            {
                var patients = await _service.GetAllAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all patients");
                return StatusCode(500, "An error occurred while retrieving patients");
            }
        }

        /// <summary>
        /// Get patient by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(Guid id)
        {
            try
            {
                var patient = await _service.GetByIdAsync(id);
                if (patient == null)
                    return NotFound($"Patient with ID {id} not found");
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patient {PatientId}", id);
                return StatusCode(500, "An error occurred while retrieving the patient");
            }
        }

        /// <summary>
        /// Search patients by name
        /// </summary>
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> Search(string name)
        {
            try
            {
                var patients = await _service.SearchByNameAsync(name);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching patients by name {Name}", name);
                return StatusCode(500, "An error occurred while searching patients");
            }
        }

        /// <summary>
        /// Create new patient
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create([FromBody] PatientCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var patient = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                return StatusCode(500, "An error occurred while creating the patient");
            }
        }

        /// <summary>
        /// Update patient
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> Update(Guid id, [FromBody] PatientUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var patient = await _service.UpdateAsync(id, dto);
                return Ok(patient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Patient with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient {PatientId}", id);
                return StatusCode(500, "An error occurred while updating the patient");
            }
        }

        /// <summary>
        /// Delete patient
        /// </summary>
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
                _logger.LogError(ex, "Error deleting patient {PatientId}", id);
                return StatusCode(500, "An error occurred while deleting the patient");
            }
        }
    }
}
