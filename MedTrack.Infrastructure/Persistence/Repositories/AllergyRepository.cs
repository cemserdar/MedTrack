using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class AllergyRepository : IAllergyRepository
    {
        private readonly ApplicationDbContext _context;
        public AllergyRepository(ApplicationDbContext context) => _context = context;

        public async Task<Allergy?> GetByIdAsync(Guid id) =>
            await _context.Allergies.FindAsync(id);

        public async Task<IEnumerable<Allergy>> GetAllAsync() =>
            await _context.Allergies.ToListAsync();

        public async Task AddAsync(Allergy allergy)
        {
            await _context.Allergies.AddAsync(allergy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Allergy allergy)
        {
            _context.Allergies.Update(allergy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy != null)
            {
                _context.Allergies.Remove(allergy);
                await _context.SaveChangesAsync();
            }
        }
    }
}