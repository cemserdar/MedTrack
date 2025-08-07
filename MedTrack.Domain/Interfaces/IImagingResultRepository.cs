using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IImagingResultRepository
    {
        Task<ImagingResult?> GetByIdAsync(Guid id);
        Task<IEnumerable<ImagingResult>> GetAllAsync();
        Task AddAsync(ImagingResult result);
        Task UpdateAsync(ImagingResult result);
        Task DeleteAsync(Guid id);
    }

}