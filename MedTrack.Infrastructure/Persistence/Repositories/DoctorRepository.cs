using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Infrastructure.Persistence.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context) => _context = context;

        public async Task<Doctor?> GetByIdAsync(Guid id) => await _context.Doctors.FindAsync(id);
        public async Task<IEnumerable<Doctor>> GetAllAsync() => await _context.Doctors.ToListAsync();
        public async Task AddAsync(Doctor doctor) { await _context.Doctors.AddAsync(doctor); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Doctor doctor) { _context.Doctors.Update(doctor); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(Guid id) { var d = await _context.Doctors.FindAsync(id); if (d != null) { _context.Doctors.Remove(d); await _context.SaveChangesAsync(); } }
    }
}