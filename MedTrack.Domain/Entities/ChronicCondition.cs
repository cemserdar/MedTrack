using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class ChronicCondition
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public DateTime DiagnosedDate { get; set; }
        public string Notes { get; set; } = string.Empty;

        public Patient? Patient { get; set; }
    }
}