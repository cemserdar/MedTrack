using AutoMapper;
using MedTrack.Application.DTOs;
using MedTrack.Application.Interfaces;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Interfaces;

namespace MedTrack.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PatientDto?> GetByIdAsync(Guid id)
        {
            var patient = await _repository.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> CreateAsync(PatientCreateDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);
            patient.Id = Guid.NewGuid();
            patient.CreatedAt = DateTime.UtcNow;
            patient.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(patient);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException($"Patient with ID {id} not found");

            _mapper.Map(dto, patient);
            patient.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(patient);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PatientDto>> SearchByNameAsync(string name)
        {
            var allPatients = await _repository.GetAllAsync();
            var filtered = allPatients.Where(p =>
                p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                (p.FirstName + " " + p.LastName).Contains(name, StringComparison.OrdinalIgnoreCase)
            );
            return _mapper.Map<IEnumerable<PatientDto>>(filtered);
        }
    }

    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DoctorDto?> GetByIdAsync(Guid id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> CreateAsync(DoctorCreateDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.Id = Guid.NewGuid();
            doctor.CreatedAt = DateTime.UtcNow;
            doctor.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(doctor);
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> UpdateAsync(Guid id, DoctorUpdateDto dto)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null) throw new KeyNotFoundException($"Doctor with ID {id} not found");

            _mapper.Map(dto, doctor);
            doctor.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(doctor);
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetBySpecialtyAsync(string specialty)
        {
            var allDoctors = await _repository.GetAllAsync();
            var filtered = allDoctors.Where(d => d.Specialty.Equals(specialty, StringComparison.OrdinalIgnoreCase));
            return _mapper.Map<IEnumerable<DoctorDto>>(filtered);
        }

        public async Task<IEnumerable<DoctorDto>> GetByClinicAsync(Guid clinicId)
        {
            var allDoctors = await _repository.GetAllAsync();
            var filtered = allDoctors.Where(d => d.ClinicId == clinicId);
            return _mapper.Map<IEnumerable<DoctorDto>>(filtered);
        }
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto?> GetByIdAsync(Guid id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto)
        {
            // Validate patient and doctor exist
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null) throw new KeyNotFoundException($"Patient with ID {dto.PatientId} not found");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null) throw new KeyNotFoundException($"Doctor with ID {dto.DoctorId} not found");

            // Check if appointment date is in the future
            if (dto.AppointmentDate < DateTime.UtcNow)
                throw new ArgumentException("Appointment date must be in the future");

            // Randevu çakışma kontrolü: Aynı doktora 30 dakika içinde başka aktif randevu verilmesini engelle
            var existingDoctorAppointments = (await _repository.GetAllAsync())
                .Where(a => a.DoctorId == dto.DoctorId && a.Status != "Cancelled");
            if (existingDoctorAppointments.Any(a => Math.Abs((a.AppointmentDate - dto.AppointmentDate).TotalMinutes) < 30))
            {
                throw new ArgumentException("Doktorun belirtilen tarih ve saat diliminde (30 dk) zaten başka bir randevusu bulunmaktadır.");
            }

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Id = Guid.NewGuid();
            appointment.Status = "Scheduled";
            appointment.CreatedAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(appointment);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> UpdateAsync(Guid id, AppointmentUpdateDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null) throw new KeyNotFoundException($"Appointment with ID {id} not found");

            _mapper.Map(dto, appointment);
            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> UpdateStatusAsync(Guid id, string status)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null) throw new KeyNotFoundException($"Appointment with ID {id} not found");

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByPatientAsync(Guid patientId)
        {
            var allAppointments = await _repository.GetAllAsync();
            var filtered = allAppointments.Where(a => a.PatientId == patientId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(filtered);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(Guid doctorId)
        {
            var allAppointments = await _repository.GetAllAsync();
            var filtered = allAppointments.Where(a => a.DoctorId == doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(filtered);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var allAppointments = await _repository.GetAllAsync();
            var filtered = allAppointments.Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate);
            return _mapper.Map<IEnumerable<AppointmentDto>>(filtered);
        }
    }
}
