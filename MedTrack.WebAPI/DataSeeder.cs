using System;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Application.Interfaces;
using MedTrack.Application.Services;
using MedTrack.Domain.Entities;
using MedTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedTrack.WebAPI
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            try
            {
                // Ensure database and schema are created
                await db.Database.EnsureCreatedAsync();

                // 1. Users
                User? adminUser = await db.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Email = "admin@medtrack.com",
                        PasswordHash = passwordHasher.HashPassword("Admin@123456"),
                        Role = "Admin",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await db.Users.AddAsync(adminUser);
                }

                User? docUser1 = await db.Users.FirstOrDefaultAsync(u => u.Username == "serdar.yilmaz");
                if (docUser1 == null)
                {
                    docUser1 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "serdar.yilmaz",
                        Email = "serdar.yilmaz@medtrack.com",
                        PasswordHash = passwordHasher.HashPassword("Doctor@123456"),
                        Role = "Doctor",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await db.Users.AddAsync(docUser1);
                }

                User? docUser2 = await db.Users.FirstOrDefaultAsync(u => u.Username == "zeynep.kaya");
                if (docUser2 == null)
                {
                    docUser2 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "zeynep.kaya",
                        Email = "zeynep.kaya@medtrack.com",
                        PasswordHash = passwordHasher.HashPassword("Doctor@123456"),
                        Role = "Doctor",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await db.Users.AddAsync(docUser2);
                }

                User? docUser3 = await db.Users.FirstOrDefaultAsync(u => u.Username == "mehmet.demir");
                if (docUser3 == null)
                {
                    docUser3 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "mehmet.demir",
                        Email = "mehmet.demir@medtrack.com",
                        PasswordHash = passwordHasher.HashPassword("Doctor@123456"),
                        Role = "Doctor",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await db.Users.AddAsync(docUser3);
                }

                User? docUser4 = await db.Users.FirstOrDefaultAsync(u => u.Username == "elif.celik");
                if (docUser4 == null)
                {
                    docUser4 = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "elif.celik",
                        Email = "elif.celik@medtrack.com",
                        PasswordHash = passwordHasher.HashPassword("Doctor@123456"),
                        Role = "Doctor",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await db.Users.AddAsync(docUser4);
                }

                await db.SaveChangesAsync();

                // 2. Reference Data: Insurance Types
                if (!await db.InsuranceTypes.AnyAsync())
                {
                    await db.InsuranceTypes.AddRangeAsync(
                        new InsuranceType { Name = "SGK (Sosyal Güvenlik Kurumu)" },
                        new InsuranceType { Name = "Tamamlayıcı Sağlık Sigortası" },
                        new InsuranceType { Name = "Özel Sağlık Sigortası (Axa / Acıbadem)" },
                        new InsuranceType { Name = "Genel Sağlık Sigortası (Yeşil Kart)" },
                        new InsuranceType { Name = "Ücretsiz / Kurum İndirimi" }
                    );
                    await db.SaveChangesAsync();
                }

                // 3. Reference Data: Diagnosis Codes
                if (!await db.DiagnosisCodes.AnyAsync())
                {
                    await db.DiagnosisCodes.AddRangeAsync(
                        new DiagnosisCode { Code = "I10", Description = "Esansiyel (primer) Hipertansiyon" },
                        new DiagnosisCode { Code = "E11", Description = "Tip 2 Diabetes Mellitus" },
                        new DiagnosisCode { Code = "J06.9", Description = "Akut Üst Solunum Yolu Enfeksiyonu" },
                        new DiagnosisCode { Code = "M54.5", Description = "Lumbago (Bel Ağrısı)" },
                        new DiagnosisCode { Code = "K21.9", Description = "Gastroözofageal Reflü Hastalığı" },
                        new DiagnosisCode { Code = "G43.9", Description = "Migren, Tanımlanmamış" }
                    );
                    await db.SaveChangesAsync();
                }

                // 4. Reference Data: Lab Test Types
                if (!await db.LabTestTypes.AnyAsync())
                {
                    await db.LabTestTypes.AddRangeAsync(
                        new LabTestType { Name = "Tam Kan Sayımı (Hemogram)", Description = "Lökosit, Eritrosit, Trombosit parametreleri" },
                        new LabTestType { Name = "Biyokimya Paneli", Description = "Glukoz, Üre, Kreatinin, AST, ALT" },
                        new LabTestType { Name = "Lipid Profili", Description = "Total Kolesterol, HDL, LDL, Trigliserid" },
                        new LabTestType { Name = "Tiroid Fonksiyon (TSH, FT4)", Description = "Tiroid hormon düzeyleri" },
                        new LabTestType { Name = "HbA1c (Glikozile Hemoglobin)", Description = "Üç aylık ortalama kan şekeri takibi" }
                    );
                    await db.SaveChangesAsync();
                }

                // 5. Reference Data: Imaging Types
                if (!await db.ImagingTypes.AnyAsync())
                {
                    await db.ImagingTypes.AddRangeAsync(
                        new ImagingType { Name = "Akciğer Grafisi (PA)", Modality = "X-Ray" },
                        new ImagingType { Name = "Manyetik Rezonans (Kranial MR)", Modality = "MRI" },
                        new ImagingType { Name = "Tüm Batın Ultrasonografisi", Modality = "USG" },
                        new ImagingType { Name = "Bilgisayarlı Tomografi (BT)", Modality = "CT" }
                    );
                    await db.SaveChangesAsync();
                }

                // 6. Clinics
                if (!await db.Clinics.AnyAsync())
                {
                    var clinics = new[]
                    {
                        new Clinic { Id = Guid.NewGuid(), Name = "Kardiyoloji Polikliniği", Location = "A Blok - Kat 2, Oda 204" },
                        new Clinic { Id = Guid.NewGuid(), Name = "Dahiliye (İç Hastalıkları)", Location = "A Blok - Kat 1, Oda 112" },
                        new Clinic { Id = Guid.NewGuid(), Name = "Nöroloji Polikliniği", Location = "B Blok - Kat 3, Oda 305" },
                        new Clinic { Id = Guid.NewGuid(), Name = "Ortopedi ve Travmatoloji", Location = "B Blok - Zemin Kat, Oda 015" },
                        new Clinic { Id = Guid.NewGuid(), Name = "Göz Hastalıkları", Location = "C Blok - Kat 1, Oda 108" }
                    };
                    await db.Clinics.AddRangeAsync(clinics);
                    await db.SaveChangesAsync();
                }

                // 7. Doctors
                var kardiyoClinic = await db.Clinics.FirstOrDefaultAsync(c => c.Name.Contains("Kardiyoloji"));
                var dahiliyeClinic = await db.Clinics.FirstOrDefaultAsync(c => c.Name.Contains("Dahiliye"));
                var norolojiClinic = await db.Clinics.FirstOrDefaultAsync(c => c.Name.Contains("Nöroloji"));
                var ortopediClinic = await db.Clinics.FirstOrDefaultAsync(c => c.Name.Contains("Ortopedi"));

                if (!await db.Doctors.AnyAsync() && kardiyoClinic != null && dahiliyeClinic != null)
                {
                    var doctors = new[]
                    {
                        new Doctor
                        {
                            Id = Guid.NewGuid(),
                            UserId = docUser1.Id,
                            FullName = "Prof. Dr. Serdar Yılmaz",
                            Specialty = "Kardiyoloji",
                            LicenseNumber = "DR-TR-34190",
                            PhoneNumber = "0532 111 2233",
                            ClinicId = kardiyoClinic.Id,
                            Biography = "Girişimsel kardiyoloji, koroner arter hastalıkları ve hipertansiyon alanında 20 yılı aşkın klinik tecrübe.",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Doctor
                        {
                            Id = Guid.NewGuid(),
                            UserId = docUser2.Id,
                            FullName = "Doç. Dr. Zeynep Kaya",
                            Specialty = "Dahiliye (İç Hastalıkları)",
                            LicenseNumber = "DR-TR-34281",
                            PhoneNumber = "0533 222 3344",
                            ClinicId = dahiliyeClinic.Id,
                            Biography = "Diyabet yönetimi, metabolik sendrom ve kronik hastalıkların takibinde uzman.",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Doctor
                        {
                            Id = Guid.NewGuid(),
                            UserId = docUser3.Id,
                            FullName = "Uzm. Dr. Mehmet Demir",
                            Specialty = "Nöroloji",
                            LicenseNumber = "DR-TR-34502",
                            PhoneNumber = "0535 333 4455",
                            ClinicId = norolojiClinic?.Id ?? kardiyoClinic.Id,
                            Biography = "Migren ve baş ağrıları, uyku bozuklukları ve EEG/EMG klinik çalışmaları.",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Doctor
                        {
                            Id = Guid.NewGuid(),
                            UserId = docUser4.Id,
                            FullName = "Op. Dr. Elif Çelik",
                            Specialty = "Ortopedi ve Travmatoloji",
                            LicenseNumber = "DR-TR-34870",
                            PhoneNumber = "0536 444 5566",
                            ClinicId = ortopediClinic?.Id ?? dahiliyeClinic.Id,
                            Biography = "Spor cerrahisi, artroskopi ve eklem kireçlenmesi cerrahisi alanında uzman.",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };
                    await db.Doctors.AddRangeAsync(doctors);
                    await db.SaveChangesAsync();
                }

                // 8. Patients
                var defaultInsurance = await db.InsuranceTypes.FirstOrDefaultAsync();
                int insuranceId = defaultInsurance?.Id ?? 1;

                if (!await db.Patients.AnyAsync())
                {
                    var patients = new[]
                    {
                        new Patient
                        {
                            Id = Guid.NewGuid(),
                            NationalId = "10293847561",
                            FirstName = "Ahmet",
                            LastName = "Yıldız",
                            Gender = 'M',
                            BirthDate = new DateTime(1982, 4, 15),
                            PhoneNumber = "0555 123 4567",
                            Email = "ahmet.yildiz@example.com",
                            Address = "Kadıköy, İstanbul",
                            EmergencyContactName = "Fatma Yıldız (Eşi)",
                            EmergencyContactPhone = "0555 987 6543",
                            InsuranceTypeId = insuranceId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Patient
                        {
                            Id = Guid.NewGuid(),
                            NationalId = "29485716302",
                            FirstName = "Ayşe",
                            LastName = "Öztürk",
                            Gender = 'F',
                            BirthDate = new DateTime(1991, 8, 22),
                            PhoneNumber = "0542 234 5678",
                            Email = "ayse.ozturk@example.com",
                            Address = "Çankaya, Ankara",
                            EmergencyContactName = "Kemal Öztürk (Babası)",
                            EmergencyContactPhone = "0542 876 5432",
                            InsuranceTypeId = insuranceId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Patient
                        {
                            Id = Guid.NewGuid(),
                            NationalId = "38592017483",
                            FirstName = "Burak",
                            LastName = "Şahin",
                            Gender = 'M',
                            BirthDate = new DateTime(1975, 11, 5),
                            PhoneNumber = "0533 345 6789",
                            Email = "burak.sahin@example.com",
                            Address = "Nilüfer, Bursa",
                            EmergencyContactName = "Selin Şahin (Kızı)",
                            EmergencyContactPhone = "0533 765 4321",
                            InsuranceTypeId = insuranceId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Patient
                        {
                            Id = Guid.NewGuid(),
                            NationalId = "47601928374",
                            FirstName = "Deniz",
                            LastName = "Aydın",
                            Gender = 'F',
                            BirthDate = new DateTime(1998, 2, 18),
                            PhoneNumber = "0505 456 7890",
                            Email = "deniz.aydin@example.com",
                            Address = "Muratpaşa, Antalya",
                            EmergencyContactName = "Merve Aydın (Kardeşi)",
                            EmergencyContactPhone = "0505 654 3210",
                            InsuranceTypeId = insuranceId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };
                    await db.Patients.AddRangeAsync(patients);
                    await db.SaveChangesAsync();
                }

                // 9. Appointments
                var sampleDoctor = await db.Doctors.FirstOrDefaultAsync();
                var samplePatient = await db.Patients.FirstOrDefaultAsync();
                var sampleClinic = await db.Clinics.FirstOrDefaultAsync();

                if (!await db.Appointments.AnyAsync() && sampleDoctor != null && samplePatient != null && sampleClinic != null)
                {
                    var today = DateTime.UtcNow.Date;
                    var appointments = new[]
                    {
                        new Appointment
                        {
                            Id = Guid.NewGuid(),
                            PatientId = samplePatient.Id,
                            DoctorId = sampleDoctor.Id,
                            ClinicId = sampleClinic.Id,
                            AppointmentDate = today.AddHours(9).AddMinutes(30),
                            Status = "Scheduled",
                            Notes = "Rutin kardiyolojik kontrol ve EKG çekimi",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Appointment
                        {
                            Id = Guid.NewGuid(),
                            PatientId = samplePatient.Id,
                            DoctorId = sampleDoctor.Id,
                            ClinicId = sampleClinic.Id,
                            AppointmentDate = today.AddHours(11).AddMinutes(0),
                            Status = "Scheduled",
                            Notes = "Tansiyon ilacı doz ayarlaması ve kan tahlili değerlendirmesi",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Appointment
                        {
                            Id = Guid.NewGuid(),
                            PatientId = samplePatient.Id,
                            DoctorId = sampleDoctor.Id,
                            ClinicId = sampleClinic.Id,
                            AppointmentDate = today.AddHours(14).AddMinutes(30),
                            Status = "Completed",
                            Notes = "Göğüs ağrısı şikayeti ile başvuru. Efor testi önerildi.",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };
                    await db.Appointments.AddRangeAsync(appointments);
                    await db.SaveChangesAsync();
                }

                logger.LogInformation("Veritabanı kontrolü ve başlangıç tohum verileri başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Veritabanı tohum verileri eklenirken bir hata oluştu.");
            }
        }
    }
}
