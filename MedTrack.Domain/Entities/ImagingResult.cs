using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class ImagingResult
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public int ImagingTypeId { get; set; }
        public DateTime ImagingDate { get; set; }
        public string ImageFilePath { get; set; } = string.Empty;
        public string RadiologistComment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Patient? Patient { get; set; }
        public ImagingType? ImagingType { get; set; }
    }
}