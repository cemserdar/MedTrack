using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context) => _context = context;

        public async Task<Patient?> GetByIdAsync(Guid id) => await _context.Patients.FindAsync(id);
        public async Task<IEnumerable<Patient>> GetAllAsync() => await _context.Patients.ToListAsync();
        public async Task AddAsync(Patient patient) { await _context.Patients.AddAsync(patient); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Patient patient) { _context.Patients.Update(patient); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(Guid id) { var p = await _context.Patients.FindAsync(id); if (p != null) { _context.Patients.Remove(p); await _context.SaveChangesAsync(); } }
    }
}