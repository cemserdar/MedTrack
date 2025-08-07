using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string PdfFilePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<PrescriptionItem>? Items { get; set; }
    }
}