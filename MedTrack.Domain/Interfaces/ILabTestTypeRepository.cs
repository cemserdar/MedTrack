using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface ILabTestTypeRepository
    {
        Task<LabTestType?> GetByIdAsync(int id);
        Task<IEnumerable<LabTestType>> GetAllAsync();
        Task AddAsync(LabTestType testType);
        Task UpdateAsync(LabTestType testType);
        Task DeleteAsync(int id);
    }
}