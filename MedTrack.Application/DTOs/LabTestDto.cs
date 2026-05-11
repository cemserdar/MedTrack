namespace MedTrack.Application.DTOs
{
    public class LabTestCreateDto
    {
        public Guid PatientId { get; set; }
        public int TestTypeId { get; set; }
        public string? Notes { get; set; }
    }

    public class LabTestUpdateDto
    {
        public string? ResultSummary { get; set; }
        public string? DoctorComment { get; set; }
    }

    public class LabTestDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public int TestTypeId { get; set; }
        public string? TestTypeName { get; set; }
        public DateTime TestDate { get; set; }
        public string ResultSummary { get; set; } = string.Empty;
        public string ResultFilePath { get; set; } = string.Empty;
        public string DoctorComment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
