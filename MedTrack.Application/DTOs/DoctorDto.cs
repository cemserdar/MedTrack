namespace MedTrack.Application.DTOs
{
    public class DoctorCreateDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid ClinicId { get; set; }
        public string Biography { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
    }

    public class DoctorUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
    }

    public class DoctorDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid ClinicId { get; set; }
        public string? ClinicName { get; set; }
        public string Biography { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
