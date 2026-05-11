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
                _logger.LogError(ex, "Error getting insurance types");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost("insurance-types")]
        public async Task<ActionResult<InsuranceTypeDto>> CreateInsuranceType([FromBody] InsuranceTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateInsuranceTypeAsync(dto);
                return CreatedAtAction(nameof(GetInsuranceTypes), type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating insurance type");
                return StatusCode(500, "An error occurred");
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
                _logger.LogError(ex, "Error getting lab test types");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost("lab-test-types")]
        public async Task<ActionResult<LabTestTypeDto>> CreateLabTestType([FromBody] LabTestTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateLabTestTypeAsync(dto);
                return CreatedAtAction(nameof(GetLabTestTypes), type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lab test type");
                return StatusCode(500, "An error occurred");
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
                _logger.LogError(ex, "Error getting imaging types");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost("imaging-types")]
        public async Task<ActionResult<ImagingTypeDto>> CreateImagingType([FromBody] ImagingTypeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _service.CreateImagingTypeAsync(dto);
                return CreatedAtAction(nameof(GetImagingTypes), type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating imaging type");
                return StatusCode(500, "An error occurred");
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
                _logger.LogError(ex, "Error getting diagnosis codes");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost("diagnosis-codes")]
        public async Task<ActionResult<DiagnosisCodeDto>> CreateDiagnosisCode([FromBody] DiagnosisCodeCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var code = await _service.CreateDiagnosisCodeAsync(dto);
                return CreatedAtAction(nameof(GetDiagnosisCodes), code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating diagnosis code");
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
