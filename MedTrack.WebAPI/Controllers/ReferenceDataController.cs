using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;

namespace MedTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IReferenceDataService _service;
        private readonly ILogger<ReferenceDataController> _logger;

        public ReferenceDataController(IReferenceDataService service, ILogger<ReferenceDataController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Insurance Types
        [HttpGet("insurance-types")]
        public async Task<ActionResult<IEnumerable<InsuranceTypeDto>>> GetInsuranceTypes()
        {
            try
            {
                var types = await _service.GetInsuranceTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sigorta türleri alınırken hata oluştu");
                return StatusCode(500, "Sigorta türleri alınırken bir hata oluştu");
            }
        }

        [HttpPost("insurance-types")]
        public async Task<ActionResult<InsuranceTypeDto>> CreateInsuranceType([FromBody] InsuranceTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateInsuranceTypeAsync(dto);
                return StatusCode(StatusCodes.Status201Created, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sigorta türü oluşturulurken hata oluştu");
                return StatusCode(500, "Sigorta türü oluşturulurken bir hata oluştu");
            }
        }

        // Lab Test Types
        [HttpGet("lab-test-types")]
        public async Task<ActionResult<IEnumerable<LabTestTypeDto>>> GetLabTestTypes()
        {
            try
            {
                var types = await _service.GetLabTestTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Laboratuvar test türleri alınırken hata oluştu");
                return StatusCode(500, "Test türleri alınırken bir hata oluştu");
            }
        }

        [HttpPost("lab-test-types")]
        public async Task<ActionResult<LabTestTypeDto>> CreateLabTestType([FromBody] LabTestTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateLabTestTypeAsync(dto);
                return StatusCode(StatusCodes.Status201Created, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Laboratuvar test türü oluşturulurken hata oluştu");
                return StatusCode(500, "Test türü oluşturulurken bir hata oluştu");
            }
        }

        // Imaging Types
        [HttpGet("imaging-types")]
        public async Task<ActionResult<IEnumerable<ImagingTypeDto>>> GetImagingTypes()
        {
            try
            {
                var types = await _service.GetImagingTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Görüntüleme türleri alınırken hata oluştu");
                return StatusCode(500, "Görüntüleme türleri alınırken bir hata oluştu");
            }
        }

        [HttpPost("imaging-types")]
        public async Task<ActionResult<ImagingTypeDto>> CreateImagingType([FromBody] ImagingTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateImagingTypeAsync(dto);
                return StatusCode(StatusCodes.Status201Created, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Görüntüleme türü oluşturulurken hata oluştu");
                return StatusCode(500, "Görüntüleme türü oluşturulurken bir hata oluştu");
            }
        }

        // Diagnosis Codes
        [HttpGet("diagnosis-codes")]
        public async Task<ActionResult<IEnumerable<DiagnosisCodeDto>>> GetDiagnosisCodes()
        {
            try
            {
                var codes = await _service.GetDiagnosisCodesAsync();
                return Ok(codes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tanı kodları alınırken hata oluştu");
                return StatusCode(500, "Tanı kodları alınırken bir hata oluştu");
            }
        }

        [HttpPost("diagnosis-codes")]
        public async Task<ActionResult<DiagnosisCodeDto>> CreateDiagnosisCode([FromBody] DiagnosisCodeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var code = await _service.CreateDiagnosisCodeAsync(dto);
                return StatusCode(StatusCodes.Status201Created, code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tanı kodu oluşturulurken hata oluştu");
                return StatusCode(500, "Tanı kodu oluşturulurken bir hata oluştu");
            }
        }
    }
}
