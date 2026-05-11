namespace MedTrack.Application.DTOs
{
    public class ClinicCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class ClinicUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class ClinicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
