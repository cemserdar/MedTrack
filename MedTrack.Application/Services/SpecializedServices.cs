using AutoMapper;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;

namespace MedTrack.Application.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly IMapper _mapper;

        public PrescriptionService(IPrescriptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PrescriptionDto?> GetByIdAsync(Guid id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            return prescription == null ? null : _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto> CreateAsync(PrescriptionCreateDto dto)
        {
            var prescription = _mapper.Map<Prescription>(dto);
            prescription.Id = Guid.NewGuid();
            prescription.PrescriptionDate = DateTime.UtcNow;
            prescription.CreatedAt = DateTime.UtcNow;
            prescription.UpdatedAt = DateTime.UtcNow;

            if (dto.Items != null && dto.Items.Any())
            {
                prescription.Items = new List<PrescriptionItem>();
                foreach (var itemDto in dto.Items)
                {
                    var item = _mapper.Map<PrescriptionItem>(itemDto);
                    item.Id = Guid.NewGuid();
                    item.PrescriptionId = prescription.Id;
                    prescription.Items.Add(item);
                }
            }

            await _repository.AddAsync(prescription);
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> UpdateAsync(Guid id, PrescriptionUpdateDto dto)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) throw new KeyNotFoundException($"Prescription with ID {id} not found");

            _mapper.Map(dto, prescription);
            prescription.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(prescription);
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetByPatientAsync(Guid patientId)
        {
            var allPrescriptions = await _repository.GetAllAsync();
            var filtered = allPrescriptions.Where(p => p.PatientId == patientId);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(filtered);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetByDoctorAsync(Guid doctorId)
        {
            var allPrescriptions = await _repository.GetAllAsync();
            var filtered = allPrescriptions.Where(p => p.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(filtered);
        }
    }

    public class MedicalNoteService : IMedicalNoteService
    {
        private readonly IMedicalNoteRepository _repository;
        private readonly IMapper _mapper;

        public MedicalNoteService(IMedicalNoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MedicalNoteDto?> GetByIdAsync(Guid id)
        {
            var note = await _repository.GetByIdAsync(id);
            return note == null ? null : _mapper.Map<MedicalNoteDto>(note);
        }

        public async Task<IEnumerable<MedicalNoteDto>> GetAllAsync()
        {
            var notes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicalNoteDto>>(notes);
        }

        public async Task<MedicalNoteDto> CreateAsync(MedicalNoteCreateDto dto)
        {
            var note = _mapper.Map<MedicalNote>(dto);
            note.Id = Guid.NewGuid();
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;
            await _repository.AddAsync(note);
            return _mapper.Map<MedicalNoteDto>(note);
        }

        public async Task<MedicalNoteDto> UpdateAsync(Guid id, MedicalNoteUpdateDto dto)
        {
            var note = await _repository.GetByIdAsync(id);
            if (note == null) throw new KeyNotFoundException($"Medical note with ID {id} not found");
            _mapper.Map(dto, note);
            note.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(note);
            return _mapper.Map<MedicalNoteDto>(note);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MedicalNoteDto>> GetByPatientAsync(Guid patientId)
        {
            var allNotes = await _repository.GetAllAsync();
            var filtered = allNotes.Where(n => n.PatientId == patientId);
            return _mapper.Map<IEnumerable<MedicalNoteDto>>(filtered);
        }

        public async Task<IEnumerable<MedicalNoteDto>> GetByDoctorAsync(Guid doctorId)
        {
            var allNotes = await _repository.GetAllAsync();
            var filtered = allNotes.Where(n => n.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<MedicalNoteDto>>(filtered);
        }
    }

    public class LabTestService : ILabTestService
    {
        private readonly ILabTestRepository _repository;
        private readonly IMapper _mapper;

        public LabTestService(ILabTestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LabTestDto?> GetByIdAsync(Guid id)
        {
            var test = await _repository.GetByIdAsync(id);
            return test == null ? null : _mapper.Map<LabTestDto>(test);
        }

        public async Task<IEnumerable<LabTestDto>> GetAllAsync()
        {
            var tests = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LabTestDto>>(tests);
        }

        public async Task<LabTestDto> CreateAsync(LabTestCreateDto dto)
        {
            var test = _mapper.Map<LabTest>(dto);
            test.Id = Guid.NewGuid();
            test.TestDate = DateTime.UtcNow;
            test.CreatedAt = DateTime.UtcNow;
            test.UpdatedAt = DateTime.UtcNow;
            await _repository.AddAsync(test);
            return _mapper.Map<LabTestDto>(test);
        }

        public async Task<LabTestDto> UpdateAsync(Guid id, LabTestUpdateDto dto)
        {
            var test = await _repository.GetByIdAsync(id);
            if (test == null) throw new KeyNotFoundException($"Lab test with ID {id} not found");
            _mapper.Map(dto, test);
            test.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(test);
            return _mapper.Map<LabTestDto>(test);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<LabTestDto>> GetByPatientAsync(Guid patientId)
        {
            var allTests = await _repository.GetAllAsync();
            var filtered = allTests.Where(t => t.PatientId == patientId);
            return _mapper.Map<IEnumerable<LabTestDto>>(filtered);
        }

        public async Task<IEnumerable<LabTestDto>> GetByDoctorAsync(Guid doctorId)
        {
            var allTests = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LabTestDto>>(allTests);
        }

        public async Task<IEnumerable<LabTestDto>> GetPendingAsync()
        {
            var allTests = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LabTestDto>>(allTests);
        }
    }

    public class ImagingService : IImagingService
    {
        private readonly IImagingResultRepository _repository;
        private readonly IMapper _mapper;

        public ImagingService(IImagingResultRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ImagingResultDto?> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result == null ? null : _mapper.Map<ImagingResultDto>(result);
        }

        public async Task<IEnumerable<ImagingResultDto>> GetAllAsync()
        {
            var results = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ImagingResultDto>>(results);
        }

        public async Task<ImagingResultDto> CreateAsync(ImagingResultCreateDto dto)
        {
            var result = _mapper.Map<ImagingResult>(dto);
            result.Id = Guid.NewGuid();
            result.ImagingDate = DateTime.UtcNow;
            result.CreatedAt = DateTime.UtcNow;
            result.UpdatedAt = DateTime.UtcNow;
            await _repository.AddAsync(result);
            return _mapper.Map<ImagingResultDto>(result);
        }

        public async Task<ImagingResultDto> UpdateAsync(Guid id, ImagingResultUpdateDto dto)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) throw new KeyNotFoundException($"Imaging result with ID {id} not found");
            _mapper.Map(dto, result);
            result.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(result);
            return _mapper.Map<ImagingResultDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ImagingResultDto>> GetByPatientAsync(Guid patientId)
        {
            var allResults = await _repository.GetAllAsync();
            var filtered = allResults.Where(r => r.PatientId == patientId);
            return _mapper.Map<IEnumerable<ImagingResultDto>>(filtered);
        }

        public async Task<IEnumerable<ImagingResultDto>> GetByDoctorAsync(Guid doctorId)
        {
            var allResults = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ImagingResultDto>>(allResults);
        }

        public async Task<IEnumerable<ImagingResultDto>> GetPendingAsync()
        {
            var allResults = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ImagingResultDto>>(allResults);
        }
    }

    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _repository;
        private readonly IMapper _mapper;

        public ClinicService(IClinicRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClinicDto?> GetByIdAsync(Guid id)
        {
            var clinic = await _repository.GetByIdAsync(id);
            return clinic == null ? null : _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<IEnumerable<ClinicDto>> GetAllAsync()
        {
            var clinics = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClinicDto>>(clinics);
        }

        public async Task<ClinicDto> CreateAsync(ClinicCreateDto dto)
        {
            var clinic = _mapper.Map<Clinic>(dto);
            clinic.Id = Guid.NewGuid();
            await _repository.AddAsync(clinic);
            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task<ClinicDto> UpdateAsync(Guid id, ClinicUpdateDto dto)
        {
            var clinic = await _repository.GetByIdAsync(id);
            if (clinic == null) throw new KeyNotFoundException($"Clinic with ID {id} not found");
            _mapper.Map(dto, clinic);
            await _repository.UpdateAsync(clinic);
            return _mapper.Map<ClinicDto>(clinic);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IInsuranceTypeRepository _insuranceTypeRepository;
        private readonly ILabTestTypeRepository _labTestTypeRepository;
        private readonly IImagingTypeRepository _imagingTypeRepository;
        private readonly IDiagnosisCodeRepository _diagnosisCodeRepository;
        private readonly IMapper _mapper;

        public ReferenceDataService(
            IInsuranceTypeRepository insuranceTypeRepository,
            ILabTestTypeRepository labTestTypeRepository,
            IImagingTypeRepository imagingTypeRepository,
            IDiagnosisCodeRepository diagnosisCodeRepository,
            IMapper mapper)
        {
            _insuranceTypeRepository = insuranceTypeRepository;
            _labTestTypeRepository = labTestTypeRepository;
            _imagingTypeRepository = imagingTypeRepository;
            _diagnosisCodeRepository = diagnosisCodeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InsuranceTypeDto>> GetInsuranceTypesAsync()
        {
            var types = await _insuranceTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InsuranceTypeDto>>(types);
        }

        public async Task<InsuranceTypeDto> CreateInsuranceTypeAsync(InsuranceTypeCreateDto dto)
        {
            var type = _mapper.Map<InsuranceType>(dto);
            await _insuranceTypeRepository.AddAsync(type);
            return _mapper.Map<InsuranceTypeDto>(type);
        }

        public async Task<IEnumerable<LabTestTypeDto>> GetLabTestTypesAsync()
        {
            var types = await _labTestTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LabTestTypeDto>>(types);
        }

        public async Task<LabTestTypeDto> CreateLabTestTypeAsync(LabTestTypeCreateDto dto)
        {
            var type = _mapper.Map<LabTestType>(dto);
            await _labTestTypeRepository.AddAsync(type);
            return _mapper.Map<LabTestTypeDto>(type);
        }

        public async Task<IEnumerable<ImagingTypeDto>> GetImagingTypesAsync()
        {
            var types = await _imagingTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ImagingTypeDto>>(types);
        }

        public async Task<ImagingTypeDto> CreateImagingTypeAsync(ImagingTypeCreateDto dto)
        {
            var type = _mapper.Map<ImagingType>(dto);
            await _imagingTypeRepository.AddAsync(type);
            return _mapper.Map<ImagingTypeDto>(type);
        }

        public async Task<IEnumerable<DiagnosisCodeDto>> GetDiagnosisCodesAsync()
        {
            var codes = await _diagnosisCodeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DiagnosisCodeDto>>(codes);
        }

        public async Task<DiagnosisCodeDto> CreateDiagnosisCodeAsync(DiagnosisCodeCreateDto dto)
        {
            var code = _mapper.Map<DiagnosisCode>(dto);
            await _diagnosisCodeRepository.AddAsync(code);
            return _mapper.Map<DiagnosisCodeDto>(code);
        }
    }
}
