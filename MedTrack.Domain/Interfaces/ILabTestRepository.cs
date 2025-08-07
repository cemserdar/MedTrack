using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface ILabTestRepository
    {
        Task<LabTest?> GetByIdAsync(Guid id);
        Task<IEnumerable<LabTest>> GetAllAsync();
        Task AddAsync(LabTest test);
        Task UpdateAsync(LabTest test);
        Task DeleteAsync(Guid id);
    }

}