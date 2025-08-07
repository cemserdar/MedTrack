using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class DiagnosisCodeRepository : IDiagnosisCodeRepository
    {
        private readonly ApplicationDbContext _context;
        public DiagnosisCodeRepository(ApplicationDbContext context) => _context = context;

        public async Task<DiagnosisCode?> GetByIdAsync(int id) => await _context.DiagnosisCodes.FindAsync(id);
        public async Task<IEnumerable<DiagnosisCode>> GetAllAsync() => await _context.DiagnosisCodes.ToListAsync();
        public async Task AddAsync(DiagnosisCode code) { await _context.DiagnosisCodes.AddAsync(code); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(DiagnosisCode code) { _context.DiagnosisCodes.Update(code); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var code = await _context.DiagnosisCodes.FindAsync(id);
            if (code != null)
            {
                _context.DiagnosisCodes.Remove(code);
                await _context.SaveChangesAsync();
            }
        }
    }
}