using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IMedicalNoteRepository
    {
        Task<MedicalNote?> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicalNote>> GetAllAsync();
        Task AddAsync(MedicalNote note);
        Task UpdateAsync(MedicalNote note);
        Task DeleteAsync(Guid id);
    }
}