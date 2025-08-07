using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IAllergyRepository
    {
        Task<Allergy?> GetByIdAsync(Guid id);
        Task<IEnumerable<Allergy>> GetAllAsync();
        Task AddAsync(Allergy allergy);
        Task UpdateAsync(Allergy allergy);
        Task DeleteAsync(Guid id);
    }

}