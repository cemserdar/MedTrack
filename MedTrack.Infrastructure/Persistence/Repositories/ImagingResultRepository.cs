using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class ImagingResultRepository : IImagingResultRepository
    {
        private readonly ApplicationDbContext _context;
        public ImagingResultRepository(ApplicationDbContext context) => _context = context;

        public async Task<ImagingResult?> GetByIdAsync(Guid id) =>
            await _context.ImagingResults.FindAsync(id);

        public async Task<IEnumerable<ImagingResult>> GetAllAsync() =>
            await _context.ImagingResults.ToListAsync();

        public async Task AddAsync(ImagingResult result)
        {
            await _context.ImagingResults.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImagingResult result)
        {
            _context.ImagingResults.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _context.ImagingResults.FindAsync(id);
            if (result != null)
            {
                _context.ImagingResults.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}