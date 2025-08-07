using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class DoctorPatientRelation
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public bool IsPrimaryDoctor { get; set; } = false;

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}