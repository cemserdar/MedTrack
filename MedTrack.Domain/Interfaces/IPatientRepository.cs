using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(Guid id);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);
    }
}