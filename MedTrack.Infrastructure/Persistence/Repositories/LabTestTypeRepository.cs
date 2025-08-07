using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class LabTestTypeRepository : ILabTestTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public LabTestTypeRepository(ApplicationDbContext context) => _context = context;

        public async Task<LabTestType?> GetByIdAsync(int id) =>
            await _context.LabTestTypes.FindAsync(id);

        public async Task<IEnumerable<LabTestType>> GetAllAsync() =>
            await _context.LabTestTypes.ToListAsync();

        public async Task AddAsync(LabTestType testType)
        {
            await _context.LabTestTypes.AddAsync(testType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LabTestType testType)
        {
            _context.LabTestTypes.Update(testType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var testType = await _context.LabTestTypes.FindAsync(id);
            if (testType != null)
            {
                _context.LabTestTypes.Remove(testType);
                await _context.SaveChangesAsync();
            }
        }
    }
}