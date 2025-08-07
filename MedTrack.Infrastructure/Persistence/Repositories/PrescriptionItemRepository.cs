using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class PrescriptionItemRepository : IPrescriptionItemRepository
    {
        private readonly ApplicationDbContext _context;
        public PrescriptionItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<PrescriptionItem?> GetByIdAsync(Guid id) =>
            await _context.PrescriptionItems.FindAsync(id);

        public async Task<IEnumerable<PrescriptionItem>> GetAllAsync() =>
            await _context.PrescriptionItems.ToListAsync();

        public async Task AddAsync(PrescriptionItem item)
        {
            await _context.PrescriptionItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PrescriptionItem item)
        {
            _context.PrescriptionItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.PrescriptionItems.FindAsync(id);
            if (item != null)
            {
                _context.PrescriptionItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}