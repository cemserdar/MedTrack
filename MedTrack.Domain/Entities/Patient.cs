using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public int InsuranceTypeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public InsuranceType? InsuranceType { get; set; }

        public ICollection<DoctorPatientRelation>? DoctorPatientRelations { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<LabTest>? LabTests { get; set; }
        public ICollection<ImagingResult>? ImagingResults { get; set; }
        public ICollection<MedicalNote>? MedicalNotes { get; set; }
        public ICollection<Allergy>? Allergies { get; set; }
        public ICollection<ChronicCondition>? ChronicConditions { get; set; }
    }
}