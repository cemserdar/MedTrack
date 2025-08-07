using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class LabTestRepository : ILabTestRepository
    {
        private readonly ApplicationDbContext _context;
        public LabTestRepository(ApplicationDbContext context) => _context = context;

        public async Task<LabTest?> GetByIdAsync(Guid id) =>
            await _context.LabTests.FindAsync(id);

        public async Task<IEnumerable<LabTest>> GetAllAsync() =>
            await _context.LabTests.ToListAsync();

        public async Task AddAsync(LabTest test)
        {
            await _context.LabTests.AddAsync(test);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LabTest test)
        {
            _context.LabTests.Update(test);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var test = await _context.LabTests.FindAsync(id);
            if (test != null)
            {
                _context.LabTests.Remove(test);
                await _context.SaveChangesAsync();
            }
        }
    }
}