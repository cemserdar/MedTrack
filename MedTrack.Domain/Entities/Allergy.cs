using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Allergy
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string Allergen { get; set; } = string.Empty;
        public string Reaction { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;

        public Patient? Patient { get; set; }
    }
}