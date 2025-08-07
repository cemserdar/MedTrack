using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class LabTest
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public int TestTypeId { get; set; }
        public DateTime TestDate { get; set; }
        public string ResultSummary { get; set; } = string.Empty;
        public string ResultFilePath { get; set; } = string.Empty;
        public string DoctorComment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Patient? Patient { get; set; }
        public LabTestType? TestType { get; set; }
    }
}