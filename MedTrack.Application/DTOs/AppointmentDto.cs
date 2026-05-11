namespace MedTrack.Application.DTOs
{
    public class AppointmentCreateDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid ClinicId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
    }

    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
    }

    public class AppointmentStatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public Guid ClinicId { get; set; }
        public string? ClinicName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
