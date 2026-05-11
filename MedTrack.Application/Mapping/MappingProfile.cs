using AutoMapper;
using MedTrack.Application.DTOs;
using MedTrack.Domain.Entities;

namespace MedTrack.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient mappings
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.InsuranceTypeName, opt => opt.MapFrom(src => src.InsuranceType!.Name));
            CreateMap<PatientCreateDto, Patient>();
            CreateMap<PatientUpdateDto, Patient>();

            // Doctor mappings
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic!.Name));
            CreateMap<DoctorCreateDto, Doctor>();
            CreateMap<DoctorUpdateDto, Doctor>();

            // Appointment mappings
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient!.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor!.FullName))
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic!.Name));
            CreateMap<AppointmentCreateDto, Appointment>();
            CreateMap<AppointmentUpdateDto, Appointment>();

            // Prescription mappings
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient!.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor!.FullName));
            CreateMap<PrescriptionCreateDto, Prescription>();
            CreateMap<PrescriptionUpdateDto, Prescription>();

            // Prescription Item mappings
            CreateMap<PrescriptionItem, PrescriptionItemDto>();
            CreateMap<PrescriptionItemCreateDto, PrescriptionItem>();

            // Medical Note mappings
            CreateMap<MedicalNote, MedicalNoteDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient!.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor!.FullName));
            CreateMap<MedicalNoteCreateDto, MedicalNote>();
            CreateMap<MedicalNoteUpdateDto, MedicalNote>();

            // Lab Test mappings
            CreateMap<LabTest, LabTestDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient!.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.TestTypeName, opt => opt.MapFrom(src => src.TestType!.Name));
            CreateMap<LabTestCreateDto, LabTest>();
            CreateMap<LabTestUpdateDto, LabTest>();

            // Imaging Result mappings
            CreateMap<ImagingResult, ImagingResultDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient!.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.ImagingTypeName, opt => opt.MapFrom(src => src.ImagingType!.Name));
            CreateMap<ImagingResultCreateDto, ImagingResult>();
            CreateMap<ImagingResultUpdateDto, ImagingResult>();

            // Clinic mappings
            CreateMap<Clinic, ClinicDto>();
            CreateMap<ClinicCreateDto, Clinic>();
            CreateMap<ClinicUpdateDto, Clinic>();

            // Reference Data mappings
            CreateMap<InsuranceType, InsuranceTypeDto>();
            CreateMap<InsuranceTypeCreateDto, InsuranceType>();

            CreateMap<LabTestType, LabTestTypeDto>();
            CreateMap<LabTestTypeCreateDto, LabTestType>();

            CreateMap<ImagingType, ImagingTypeDto>();
            CreateMap<ImagingTypeCreateDto, ImagingType>();

            CreateMap<DiagnosisCode, DiagnosisCodeDto>();
            CreateMap<DiagnosisCodeCreateDto, DiagnosisCode>();
        }
    }
}
