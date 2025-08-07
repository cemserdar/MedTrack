using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class DoctorPatientRelationRepository : IDoctorPatientRelationRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorPatientRelationRepository(ApplicationDbContext context) => _context = context;

        public async Task<DoctorPatientRelation?> GetByIdAsync(Guid id) =>
            await _context.DoctorPatientRelations.FindAsync(id);

        public async Task<IEnumerable<DoctorPatientRelation>> GetAllAsync() =>
            await _context.DoctorPatientRelations.ToListAsync();

        public async Task AddAsync(DoctorPatientRelation relation)
        {
            await _context.DoctorPatientRelations.AddAsync(relation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DoctorPatientRelation relation)
        {
            _context.DoctorPatientRelations.Update(relation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var relation = await _context.DoctorPatientRelations.FindAsync(id);
            if (relation != null)
            {
                _context.DoctorPatientRelations.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }
    }
}