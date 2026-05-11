using MedTrack.Application.DTOs;

namespace MedTrack.Application.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto> CreateAsync(PatientCreateDto dto);
        Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<PatientDto>> SearchByNameAsync(string name);
    }

    public interface IDoctorService
    {
        Task<DoctorDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto> CreateAsync(DoctorCreateDto dto);
        Task<DoctorDto> UpdateAsync(Guid id, DoctorUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<DoctorDto>> GetBySpecialtyAsync(string specialty);
        Task<IEnumerable<DoctorDto>> GetByClinicAsync(Guid clinicId);
    }

    public interface IAppointmentService
    {
        Task<AppointmentDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto);
        Task<AppointmentDto> UpdateAsync(Guid id, AppointmentUpdateDto dto);
        Task<AppointmentDto> UpdateStatusAsync(Guid id, string status);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<AppointmentDto>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(Guid doctorId);
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }

    public interface IPrescriptionService
    {
        Task<PrescriptionDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto> CreateAsync(PrescriptionCreateDto dto);
        Task<PrescriptionDto> UpdateAsync(Guid id, PrescriptionUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<PrescriptionDto>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<PrescriptionDto>> GetByDoctorAsync(Guid doctorId);
    }

    public interface IMedicalNoteService
    {
        Task<MedicalNoteDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicalNoteDto>> GetAllAsync();
        Task<MedicalNoteDto> CreateAsync(MedicalNoteCreateDto dto);
        Task<MedicalNoteDto> UpdateAsync(Guid id, MedicalNoteUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<MedicalNoteDto>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<MedicalNoteDto>> GetByDoctorAsync(Guid doctorId);
    }

    public interface ILabTestService
    {
        Task<LabTestDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<LabTestDto>> GetAllAsync();
        Task<LabTestDto> CreateAsync(LabTestCreateDto dto);
        Task<LabTestDto> UpdateAsync(Guid id, LabTestUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<LabTestDto>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<LabTestDto>> GetByDoctorAsync(Guid doctorId);
        Task<IEnumerable<LabTestDto>> GetPendingAsync();
    }

    public interface IImagingService
    {
        Task<ImagingResultDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ImagingResultDto>> GetAllAsync();
        Task<ImagingResultDto> CreateAsync(ImagingResultCreateDto dto);
        Task<ImagingResultDto> UpdateAsync(Guid id, ImagingResultUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ImagingResultDto>> GetByPatientAsync(Guid patientId);
        Task<IEnumerable<ImagingResultDto>> GetByDoctorAsync(Guid doctorId);
        Task<IEnumerable<ImagingResultDto>> GetPendingAsync();
    }

    public interface IClinicService
    {
        Task<ClinicDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ClinicDto>> GetAllAsync();
        Task<ClinicDto> CreateAsync(ClinicCreateDto dto);
        Task<ClinicDto> UpdateAsync(Guid id, ClinicUpdateDto dto);
        Task DeleteAsync(Guid id);
    }

    public interface IReferenceDataService
    {
        // Insurance Types
        Task<IEnumerable<InsuranceTypeDto>> GetInsuranceTypesAsync();
        Task<InsuranceTypeDto> CreateInsuranceTypeAsync(InsuranceTypeCreateDto dto);

        // Lab Test Types
        Task<IEnumerable<LabTestTypeDto>> GetLabTestTypesAsync();
        Task<LabTestTypeDto> CreateLabTestTypeAsync(LabTestTypeCreateDto dto);

        // Imaging Types
        Task<IEnumerable<ImagingTypeDto>> GetImagingTypesAsync();
        Task<ImagingTypeDto> CreateImagingTypeAsync(ImagingTypeCreateDto dto);

        // Diagnosis Codes
        Task<IEnumerable<DiagnosisCodeDto>> GetDiagnosisCodesAsync();
        Task<DiagnosisCodeDto> CreateDiagnosisCodeAsync(DiagnosisCodeCreateDto dto);
    }
}
