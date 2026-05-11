namespace MedTrack.Application.DTOs
{
    public class PatientCreateDto
    {
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
    }

    public class PatientUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public int InsuranceTypeId { get; set; }
    }

    public class PatientDto
    {
        public Guid Id { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public int InsuranceTypeId { get; set; }
        public string? InsuranceTypeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
