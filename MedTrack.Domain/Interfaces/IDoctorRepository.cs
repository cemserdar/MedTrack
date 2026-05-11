using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByIdAsync(Guid id);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(Guid id);
    }
}