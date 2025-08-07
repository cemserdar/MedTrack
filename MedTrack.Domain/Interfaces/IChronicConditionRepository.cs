using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IChronicConditionRepository
    {
        Task<ChronicCondition?> GetByIdAsync(Guid id);
        Task<IEnumerable<ChronicCondition>> GetAllAsync();
        Task AddAsync(ChronicCondition condition);
        Task UpdateAsync(ChronicCondition condition);
        Task DeleteAsync(Guid id);
    }

}