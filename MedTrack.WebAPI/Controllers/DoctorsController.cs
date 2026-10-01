using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(IDoctorService service, ILogger<DoctorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all doctors
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
        {
            try
            {
                var doctors = await _service.GetAllAsync();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all doctors");
                return StatusCode(500, "An error occurred while retrieving doctors");
            }
        }

        /// <summary>
        /// Get doctor by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetById(Guid id)
        {
            try
            {
                var doctor = await _service.GetByIdAsync(id);
                if (doctor == null)
                    return NotFound($"Doctor with ID {id} not found");
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctor {DoctorId}", id);
                return StatusCode(500, "An error occurred while retrieving the doctor");
            }
        }

        /// <summary>
        /// Get doctors by specialty
        /// </summary>
        [HttpGet("specialty/{specialty}")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetBySpecialty(string specialty)
        {
            try
            {
                var doctors = await _service.GetBySpecialtyAsync(specialty);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctors by specialty {Specialty}", specialty);
                return StatusCode(500, "An error occurred while retrieving doctors");
            }
        }

        /// <summary>
        /// Get doctors by clinic
        /// </summary>
        [HttpGet("clinic/{clinicId}")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetByClinic(Guid clinicId)
        {
            try
            {
                var doctors = await _service.GetByClinicAsync(clinicId);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctors by clinic {ClinicId}", clinicId);
                return StatusCode(500, "An error occurred while retrieving doctors");
            }
        }

        /// <summary>
        /// Create new doctor
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DoctorDto>> Create([FromBody] DoctorCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var doctor = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor");
                return StatusCode(500, "An error occurred while creating the doctor");
            }
        }

        /// <summary>
        /// Update doctor
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorDto>> Update(Guid id, [FromBody] DoctorUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var doctor = await _service.UpdateAsync(id, dto);
                return Ok(doctor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Doctor with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor {DoctorId}", id);
                return StatusCode(500, "An error occurred while updating the doctor");
            }
        }

        /// <summary>
        /// Delete doctor
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound($"Doctor with ID {id} not found");

                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Doctor with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor {DoctorId}", id);
                return StatusCode(500, "An error occurred while deleting the doctor");
            }
        }
    }
}
