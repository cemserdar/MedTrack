namespace MedTrack.Application.DTOs
{
    public class ImagingResultCreateDto
    {
        public Guid PatientId { get; set; }
        public int ImagingTypeId { get; set; }
        public string? Notes { get; set; }
    }

    public class ImagingResultUpdateDto
    {
        public string? RadiologistComment { get; set; }
    }

    public class ImagingResultDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public int ImagingTypeId { get; set; }
        public string? ImagingTypeName { get; set; }
        public DateTime ImagingDate { get; set; }
        public string ImageFilePath { get; set; } = string.Empty;
        public string RadiologistComment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
