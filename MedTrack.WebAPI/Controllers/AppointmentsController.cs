using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;
using MedTrack.WebAPI.Hubs;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly IHubContext<MedicalHub> _hubContext;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(
            IAppointmentService service,
            IHubContext<MedicalHub> hubContext,
            ILogger<AppointmentsController> logger)
        {
            _service = service;
            _hubContext = hubContext;
            _logger = logger;
        }

        /// <summary>
        /// Tüm randevuları listele
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
                _logger.LogError(ex, "Randevular alınırken hata oluştu");
                return StatusCode(500, "Randevular alınırken bir hata oluştu");
            }
        }

        /// <summary>
        /// ID'ye göre randevu detayı
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
        {
            try
            {
                var appointment = await _service.GetByIdAsync(id);
                if (appointment == null)
                    return NotFound($"ID'si {id} olan randevu bulunamadı");
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu {AppointmentId} alınırken hata oluştu", id);
                return StatusCode(500, "Randevu getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Hastanın randevuları
        /// </summary>
        [HttpGet("patient/{patientId:guid}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPatient(Guid patientId)
        {
            try
            {
                var appointments = await _service.GetByPatientAsync(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hasta {PatientId} randevuları alınırken hata", patientId);
                return StatusCode(500, "Hasta randevuları alınırken bir hata oluştu");
            }
        }

        /// <summary>
        /// Doktorun randevuları
        /// </summary>
        [HttpGet("doctor/{doctorId:guid}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDoctor(Guid doctorId)
        {
            try
            {
                var appointments = await _service.GetByDoctorAsync(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doktor {DoctorId} randevuları alınırken hata", doctorId);
                return StatusCode(500, "Doktor randevuları alınırken bir hata oluştu");
            }
        }

        /// <summary>
        /// Tarih aralığına göre randevular
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (endDate < startDate)
                    return BadRequest("Bitiş tarihi başlangıç tarihinden önce olamaz.");

                var appointments = await _service.GetByDateRangeAsync(startDate, endDate);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tarih aralığına göre randevular alınırken hata");
                return StatusCode(500, "Randevular alınırken bir hata oluştu");
            }
        }

        /// <summary>
        /// Yeni randevu oluştur
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] AppointmentCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.CreateAsync(dto);

                // Gerçek zamanlı SignalR bildirimi
                await _hubContext.Clients.Group($"Doctor_{dto.DoctorId}").SendAsync("AppointmentCreated", appointment);
                await _hubContext.Clients.Group($"Patient_{dto.PatientId}").SendAsync("AppointmentCreated", appointment);
                await _hubContext.Clients.All.SendAsync("AppointmentCreatedBroadcast", new { id = appointment.Id, date = appointment.AppointmentDate });

                return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu oluşturulurken hata");
                return StatusCode(500, "Randevu oluşturulurken bir hata oluştu");
            }
        }

        /// <summary>
        /// Randevu güncelle
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AppointmentDto>> Update(Guid id, [FromBody] AppointmentUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.UpdateAsync(id, dto);

                // Gerçek zamanlı SignalR bildirimi
                await _hubContext.Clients.All.SendAsync("AppointmentUpdated", appointment);

                return Ok(appointment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"ID'si {id} olan randevu bulunamadı");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu {AppointmentId} güncellenirken hata", id);
                return StatusCode(500, "Randevu güncellenirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Randevu durumunu güncelle (Scheduled, Completed, Cancelled)
        /// </summary>
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<AppointmentDto>> UpdateStatus(Guid id, [FromBody] AppointmentStatusUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _service.UpdateStatusAsync(id, dto.Status);

                // Gerçek zamanlı SignalR bildirimi
                await _hubContext.Clients.All.SendAsync("AppointmentStatusUpdated", new { id = appointment.Id, status = appointment.Status });

                return Ok(appointment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"ID'si {id} olan randevu bulunamadı");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu durumu {AppointmentId} güncellenirken hata", id);
                return StatusCode(500, "Randevu durumu güncellenirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Randevu sil / iptal
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound($"ID'si {id} olan randevu bulunamadı");

                await _service.DeleteAsync(id);

                // Gerçek zamanlı SignalR bildirimi
                await _hubContext.Clients.All.SendAsync("AppointmentCancelled", id.ToString());

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"ID'si {id} olan randevu bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Randevu {AppointmentId} silinirken hata", id);
                return StatusCode(500, "Randevu silinirken bir hata oluştu");
            }
        }
    }
}
