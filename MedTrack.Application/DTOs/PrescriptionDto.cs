namespace MedTrack.Application.DTOs
{
    public class PrescriptionItemDto
    {
        public Guid Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? Instructions { get; set; }
    }

    public class PrescriptionItemCreateDto
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? Instructions { get; set; }
    }

    public class PrescriptionCreateDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string? Diagnosis { get; set; }
        public List<PrescriptionItemCreateDto> Items { get; set; } = new();
    }

    public class PrescriptionUpdateDto
    {
        public string? Diagnosis { get; set; }
    }

    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? Diagnosis { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
