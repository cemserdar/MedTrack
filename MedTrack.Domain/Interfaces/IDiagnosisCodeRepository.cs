using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IDiagnosisCodeRepository
    {
        Task<DiagnosisCode?> GetByIdAsync(int id);
        Task<IEnumerable<DiagnosisCode>> GetAllAsync();
        Task AddAsync(DiagnosisCode code);
        Task UpdateAsync(DiagnosisCode code);
        Task DeleteAsync(int id);
    }
}