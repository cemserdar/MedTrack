using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IDoctorPatientRelationRepository
    {
        Task<DoctorPatientRelation?> GetByIdAsync(Guid id);
        Task<IEnumerable<DoctorPatientRelation>> GetAllAsync();
        Task AddAsync(DoctorPatientRelation relation);
        Task UpdateAsync(DoctorPatientRelation relation);
        Task DeleteAsync(Guid id);
    }
}