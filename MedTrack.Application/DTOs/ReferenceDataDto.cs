namespace MedTrack.Application.DTOs
{
    // Insurance Type DTOs
    public class InsuranceTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class InsuranceTypeCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }

    // Lab Test Type DTOs
    public class LabTestTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class LabTestTypeCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // Imaging Type DTOs
    public class ImagingTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Modality { get; set; } = string.Empty;
    }

    public class ImagingTypeCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Modality { get; set; } = string.Empty;
    }

    // Diagnosis Code DTOs
    public class DiagnosisCodeDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class DiagnosisCodeCreateDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
