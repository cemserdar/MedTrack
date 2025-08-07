using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class MedicalNoteRepository : IMedicalNoteRepository
    {
        private readonly ApplicationDbContext _context;
        public MedicalNoteRepository(ApplicationDbContext context) => _context = context;

        public async Task<MedicalNote?> GetByIdAsync(Guid id) =>
            await _context.MedicalNotes.Include(n => n.Patient).Include(n => n.Doctor).FirstOrDefaultAsync(n => n.Id == id);

        public async Task<IEnumerable<MedicalNote>> GetAllAsync() =>
            await _context.MedicalNotes.Include(n => n.Patient).Include(n => n.Doctor).ToListAsync();

        public async Task AddAsync(MedicalNote note)
        {
            await _context.MedicalNotes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MedicalNote note)
        {
            _context.MedicalNotes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var note = await _context.MedicalNotes.FindAsync(id);
            if (note != null)
            {
                _context.MedicalNotes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }
}