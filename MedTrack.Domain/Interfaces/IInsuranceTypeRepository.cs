using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IInsuranceTypeRepository
    {
        Task<InsuranceType?> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceType>> GetAllAsync();
        Task AddAsync(InsuranceType insuranceType);
        Task UpdateAsync(InsuranceType insuranceType);
        Task DeleteAsync(int id);
    }
}