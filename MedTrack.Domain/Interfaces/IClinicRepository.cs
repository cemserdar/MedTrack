using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IClinicRepository
    {
        Task<Clinic?> GetByIdAsync(Guid id);
        Task<IEnumerable<Clinic>> GetAllAsync();
        Task AddAsync(Clinic clinic);
        Task UpdateAsync(Clinic clinic);
        Task DeleteAsync(Guid id);
    }
}