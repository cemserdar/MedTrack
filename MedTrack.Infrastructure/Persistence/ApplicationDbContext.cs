using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedTrack.Infrastructure.Persistence
{
    using MedTrack.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ChronicCondition> ChronicConditions { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<DiagnosisCode> DiagnosisCodes { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorPatientRelation> DoctorPatientRelations { get; set; }
        public DbSet<ImagingResult> ImagingResults { get; set; }
        public DbSet<ImagingType> ImagingTypes { get; set; }
        public DbSet<InsuranceType> InsuranceTypes { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestType> LabTestTypes { get; set; }
        public DbSet<MedicalNote> MedicalNotes { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary Key: DoctorPatientRelation uses its own Guid Id
            modelBuilder.Entity<DoctorPatientRelation>()
                .HasKey(dpr => dpr.Id);

            // Prevent duplicate active relations between the same doctor and patient
            modelBuilder.Entity<DoctorPatientRelation>()
                .HasIndex(dpr => new { dpr.DoctorId, dpr.PatientId })
                .IsUnique();

            modelBuilder.Entity<DoctorPatientRelation>()
                .HasOne(dpr => dpr.Doctor)
                .WithMany(d => d.DoctorPatientRelations)
                .HasForeignKey(dpr => dpr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorPatientRelation>()
                .HasOne(dpr => dpr.Patient)
                .WithMany(p => p.DoctorPatientRelations)
                .HasForeignKey(dpr => dpr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }



}
