using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IPrescriptionItemRepository
    {
        Task<PrescriptionItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<PrescriptionItem>> GetAllAsync();
        Task AddAsync(PrescriptionItem item);
        Task UpdateAsync(PrescriptionItem item);
        Task DeleteAsync(Guid id);
    }
}