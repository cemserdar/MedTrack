using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IImagingTypeRepository
    {
        Task<ImagingType?> GetByIdAsync(int id);
        Task<IEnumerable<ImagingType>> GetAllAsync();
        Task AddAsync(ImagingType imagingType);
        Task UpdateAsync(ImagingType imagingType);
        Task DeleteAsync(int id);
    }
}