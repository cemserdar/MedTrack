using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class InsuranceTypeRepository : IInsuranceTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public InsuranceTypeRepository(ApplicationDbContext context) => _context = context;

        public async Task<InsuranceType?> GetByIdAsync(int id) =>
            await _context.InsuranceTypes.FindAsync(id);

        public async Task<IEnumerable<InsuranceType>> GetAllAsync() =>
            await _context.InsuranceTypes.ToListAsync();

        public async Task AddAsync(InsuranceType insuranceType)
        {
            await _context.InsuranceTypes.AddAsync(insuranceType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InsuranceType insuranceType)
        {
            _context.InsuranceTypes.Update(insuranceType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var insuranceType = await _context.InsuranceTypes.FindAsync(id);
            if (insuranceType != null)
            {
                _context.InsuranceTypes.Remove(insuranceType);
                await _context.SaveChangesAsync();
            }
        }
    }
}