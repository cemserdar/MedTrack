using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class ChronicConditionRepository : IChronicConditionRepository
    {
        private readonly ApplicationDbContext _context;
        public ChronicConditionRepository(ApplicationDbContext context) => _context = context;

        public async Task<ChronicCondition?> GetByIdAsync(Guid id) =>
            await _context.ChronicConditions.FindAsync(id);

        public async Task<IEnumerable<ChronicCondition>> GetAllAsync() =>
            await _context.ChronicConditions.ToListAsync();

        public async Task AddAsync(ChronicCondition condition)
        {
            await _context.ChronicConditions.AddAsync(condition);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChronicCondition condition)
        {
            _context.ChronicConditions.Update(condition);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var condition = await _context.ChronicConditions.FindAsync(id);
            if (condition != null)
            {
                _context.ChronicConditions.Remove(condition);
                await _context.SaveChangesAsync();
            }
        }
    }
}