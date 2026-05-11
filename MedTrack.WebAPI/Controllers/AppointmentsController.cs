using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentService service, ILogger<AppointmentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            try
            {
                var appointments = await _service.GetAllAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all appointments");
                return StatusCode(500, "An error occurred while retrieving appointments");
            }
        }

        /// <summary>
        /// Get appointment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
        {
            try
            {
                var appointment = await _service.GetByIdAsync(id);
                if (appointment == null)
                    return NotFound($"Appointment with ID {id} not found");
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting appointment {AppointmentId}", id);
                return StatusCode(500, "An error occurred while retrieving the appointment");
            }
        }

        /// <summary>
        /// Get appointments by patient
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var appointments = await _service.GetByPatientAsync(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting appointments for patient {PatientId}", patientId);
                return StatusCode(500, "An error occurred while retrieving appointments");
            }
        }

        /// <summary>
        /// Get appointments by doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var appointments = await _service.GetByDoctorAsync(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting appointments for doctor {DoctorId}", doctorId);
                return StatusCode(500, "An error occurred while retrieving appointments");
            }
        }

        /// <summary>
        /// Get appointments by date range
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var appointments = await _service.GetByDateRangeAsync(startDate, endDate);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting appointments by date range");
                return StatusCode(500, "An error occurred while retrieving appointments");
            }
        }

        /// <summary>
        /// Create new appointment
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] AppointmentCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment");
                return StatusCode(500, "An error occurred while creating the appointment");
            }
        }

        /// <summary>
        /// Update appointment
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> Update(Guid id, [FromBody] AppointmentUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.UpdateAsync(id, dto);
                return Ok(appointment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Appointment with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment {AppointmentId}", id);
                return StatusCode(500, "An error occurred while updating the appointment");
            }
        }

        /// <summary>
        /// Update appointment status
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<AppointmentDto>> UpdateStatus(Guid id, [FromBody] AppointmentStatusUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.UpdateStatusAsync(id, dto.Status);
                return Ok(appointment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Appointment with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment status {AppointmentId}", id);
                return StatusCode(500, "An error occurred while updating the appointment status");
            }
        }

        /// <summary>
        /// Delete appointment
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
                _logger.LogError(ex, "Error deleting appointment {AppointmentId}", id);
                return StatusCode(500, "An error occurred while deleting the appointment");
            }
        }
    }
}
