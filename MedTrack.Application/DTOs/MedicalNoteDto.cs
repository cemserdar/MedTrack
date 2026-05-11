namespace MedTrack.Application.DTOs
{
    public class MedicalNoteCreateDto
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string NoteContent { get; set; } = string.Empty;
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
    }

    public class MedicalNoteUpdateDto
    {
        public string NoteContent { get; set; } = string.Empty;
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
    }

    public class MedicalNoteDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string NoteContent { get; set; } = string.Empty;
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
