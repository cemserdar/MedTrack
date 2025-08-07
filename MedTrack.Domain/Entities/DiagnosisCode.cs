namespace MedTrack.Domain.Entities
{
    public class DiagnosisCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}