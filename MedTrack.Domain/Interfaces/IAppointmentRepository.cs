using MedTrack.Domain.Entities;

namespace MedTrack.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Guid id);
    }
}
