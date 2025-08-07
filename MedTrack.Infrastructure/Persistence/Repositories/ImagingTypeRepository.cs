using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class ImagingTypeRepository : IImagingTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ImagingTypeRepository(ApplicationDbContext context) => _context = context;

        public async Task<ImagingType?> GetByIdAsync(int id) =>
            await _context.ImagingTypes.FindAsync(id);

        public async Task<IEnumerable<ImagingType>> GetAllAsync() =>
            await _context.ImagingTypes.ToListAsync();

        public async Task AddAsync(ImagingType imagingType)
        {
            await _context.ImagingTypes.AddAsync(imagingType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImagingType imagingType)
        {
            _context.ImagingTypes.Update(imagingType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var imagingType = await _context.ImagingTypes.FindAsync(id);
            if (imagingType != null)
            {
                _context.ImagingTypes.Remove(imagingType);
                await _context.SaveChangesAsync();
            }
        }
    }
}