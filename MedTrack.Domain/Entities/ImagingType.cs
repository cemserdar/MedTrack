using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class ImagingType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Modality { get; set; } = string.Empty;

        public ICollection<ImagingResult>? ImagingResults { get; set; }
    }
}