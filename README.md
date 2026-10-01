# 🏥 MedTrack — Medical Tracking & Appointment Management System

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13-239120?style=flat&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=flat)](https://docs.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC292B?style=flat&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![SignalR](https://img.shields.io/badge/SignalR-Real--Time-0078D7?style=flat)](https://dotnet.microsoft.com/apps/aspnet/signalr)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat&logo=docker)](https://www.docker.com/)
[![Swagger](https://img.shields.io/badge/OpenAPI-Swagger-85EA2D?style=flat&logo=swagger)](https://swagger.io/)

MedTrack, modern sağlık kuruluşları, hastaneler ve poliklinikler için geliştirilmiş, yüksek performanslı, kurumsal mimari standartlarına uygun bir **Tıbbi Takip ve Randevu Yönetim Sistemi** REST API'sidir.

---

## 🌟 Öne Çıkan Özellikler

- **Clean / N-Tier Architecture**: Domain, Application, Infrastructure ve WebAPI katmanları ile tam sorumluluk ayrımı (Separation of Concerns).
- **Hasta & Doktor Yönetimi**: Kimlik, iletişim, branş, sigorta ve geçmiş kayıtları yönetimi.
- **Akıllı Randevu Sistemi**: Çakışma önleyici (double-booking prevention) 30 dakikalık aralık kontrolü, durum güncellemeleri ve tarih filtreleme.
- **Gerçek Zamanlı Bildirimler (SignalR)**: Randevu oluşturulduğunda, güncellendiğinde veya iptal edildiğinde WebSocket üzerinden anlık tetikleme.
- **Tıbbi Kayıtlar**:
  - Reçete & Çoklu İlaç Kalemleri
  - Laboratuvar Testleri & Durum Takibi
  - Radyoloji / Görüntüleme Sonuçları (MR, BT, Röntgen vb.)
  - Doktor Tıbbi Notları & Tanı Kodları
- **Güvenli Kimlik Doğrulama**:
  - PBKDF2 (100.000 iterasyon + Salt) şifre özetleme
  - Rol bazlı JWT (Admin, Doctor, Staff, Patient)
  - SignalR WebSocket query-string token desteği
- **Container Desteği**: Docker ve Docker Compose ile SQL Server dahil tek komutla ayağa kalkabilen yapı.
- **Health Checks**: `/health/status`, `/health/live`, `/health/ready` liveness/readiness endpoint'leri.

---

## 🏛 Mimari Yapı

```
MedTrack/
├── MedTrack.Domain/          # Çekirdek Varlıklar (Entities) & Repository Arayüzleri
│   ├── Entities/             # 17 Veritabanı Varlığı
│   └── Interfaces/           # 17 Repository Interface'i
├── MedTrack.Application/     # İş Mantığı, Servisler, DTO'lar, Eşlemeler
│   ├── DTOs/                 # Veri Transfer Nesneleri (Validation etiketleri ile)
│   ├── Interfaces/           # Servis Arayüzleri
│   ├── Mapping/              # AutoMapper Profilleri
│   └── Services/             # Servis Uygulamaları, JWT Servisi, PBKDF2 Hasher
├── MedTrack.Infrastructure/  # EF Core, Veritabanı Erişimi, Konfigürasyon
│   ├── Migrations/           # EF Core Migration Dosyaları
│   └── Persistence/          # DbContext ve Repository Implementasyonları
└── MedTrack.WebAPI/          # REST Controller'lar, SignalR Hub, Middleware
    ├── Controllers/          # 10 REST Controller (50+ Endpoint)
    ├── Hubs/                 # SignalR MedicalHub (WebSocket)
    └── Program.cs            # DI Container, Pipeline & CORS Yapılandırması
```

---

## 🚀 Hızlı Başlangıç

### Gereksinimler

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) veya Docker Desktop

### 1. Yerel Ortamda Çalıştırma

1. **Repoyu klonlayın:**
   ```bash
   git clone https://github.com/kullanici-adi/MedTrack.git
   cd MedTrack
   ```

2. **Veritabanı bağlantı dizesini kontrol edin (`appsettings.json`):**
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=MedTrack;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Veritabanı migration'larını uygulayın:**
   ```bash
   dotnet ef database update --project MedTrack.Infrastructure --startup-project MedTrack.WebAPI
   ```

4. **Projeyi derleyin ve çalıştırın:**
   ```bash
   dotnet run --project MedTrack.WebAPI
   ```

5. **Swagger UI'ı açın:**
   Tarayıcınızdan `http://localhost:5274` adresine gidin.

---

### 2. Docker ile Çalıştırma

Tüm sistemi (SQL Server + Web API) tek bir komutla ayağa kaldırabilirsiniz:

```bash
docker-compose up -d
```

- **API & Swagger**: `http://localhost:5000`
- **Health Check**: `http://localhost:5000/health/status`
- **SQL Server Portu**: `localhost:1433`

Kapatmak için:
```bash
docker-compose down
```

---

## 🔑 Varsayılan Kullanıcı Girişleri (Seed)

Sistem ilk açıldığında otomatik olarak aşağıdaki varsayılan kullanıcılar oluşturulur:

| Kullanıcı Adı | Şifre | Rol |
|---|---|---|
| `admin` | `Admin@123456` | Admin |
| `doctor` | `Doctor@123456` | Doctor |

> **JWT Kullanımı**: `/api/auth/login` endpoint'inden token aldıktan sonra Swagger UI'da sağ üstteki **Authorize** butonuna tıklayıp `Bearer <TOKEN>` yazarak tüm korumalı uçları çağırabilirsiniz.

---

## 📡 API Endpoint Özeti

### 🔐 Kimlik Doğrulama (`/api/auth`)
- `POST /api/auth/login` — Kullanıcı girişi & JWT üretimi
- `POST /api/auth/register` — Yeni kullanıcı kaydı (PBKDF2 şifreleme ile)
- `POST /api/auth/refresh` — Token tazeleme

### 👤 Hasta Yönetimi (`/api/patients`)
- `GET /api/patients` — Tüm hastaları listele
- `GET /api/patients/{id}` — ID ile hasta detayı
- `GET /api/patients/search/{name}` — İsme göre hasta arama
- `POST /api/patients` — Yeni hasta kaydı
- `PUT /api/patients/{id}` — Hasta bilgisi güncelleme
- `DELETE /api/patients/{id}` — Hasta kaydını silme

### 🩺 Doktor Yönetimi (`/api/doctors`)
- `GET /api/doctors` — Tüm doktorları listele
- `GET /api/doctors/{id}` — ID ile doktor detayı
- `GET /api/doctors/specialty/{specialty}` — Uzmanlığa göre doktorlar
- `GET /api/doctors/clinic/{clinicId}` — Kliniğe göre doktorlar
- `POST /api/doctors` — Yeni doktor ekle
- `PUT /api/doctors/{id}` — Doktor güncelle
- `DELETE /api/doctors/{id}` — Doktor kaydını sil

### 📅 Randevular (`/api/appointments`)
- `GET /api/appointments` — Tüm randevular
- `GET /api/appointments/{id}` — Randevu detayı
- `GET /api/appointments/patient/{patientId}` — Hastanın randevuları
- `GET /api/appointments/doctor/{doctorId}` — Doktorun randevuları
- `GET /api/appointments/date-range` — Tarih aralığına göre randevular
- `POST /api/appointments` — Randevu oluştur *(Çakışma kontrolü & SignalR anlık bildirimi)*
- `PUT /api/appointments/{id}` — Randevu güncelle *(SignalR anlık bildirimi)*
- `PATCH /api/appointments/{id}/status` — Durum güncelle (Scheduled, Completed, Cancelled)
- `DELETE /api/appointments/{id}` — Randevu iptal/sil

### 💊 Tıbbi Kayıtlar
- **Reçeteler**: `/api/prescriptions` [GET, POST, PUT, DELETE]
- **Tıbbi Notlar**: `/api/medicalnotes` [GET, POST, PUT, DELETE]
- **Laboratuvar Testleri**: `/api/labtests` [GET, POST, PUT, DELETE, GET /pending]
- **Görüntüleme / Radyoloji**: `/api/imaging` [GET, POST, PUT, DELETE, GET /pending]
- **Klinikler**: `/api/clinics` [GET, POST, PUT, DELETE]
- **Referans Veriler**: `/api/referencedata` (Sigorta, Test Türleri, Görüntüleme Türleri, ICD-10 Tanı Kodları)

---

## ⚡ SignalR Gerçek Zamanlı Olaylar

WebSocket Hub Adresi: `/hubs/medical`

İstemciler aşağıdaki olayları dinleyebilir:
- `AppointmentCreated`: Yeni bir randevu alındığında tetiklenir
- `AppointmentUpdated`: Randevu güncellendiğinde tetiklenir
- `AppointmentStatusUpdated`: Randevu durumu değiştiğinde tetiklenir
- `AppointmentCancelled`: Randevu iptal edildiğinde tetiklenir
- `UserConnected` / `UserDisconnected`: Kullanıcı varlık bildirimleri

---

## 🛡 Güvenlik ve Mimari Prensipler

- **Şifreleme**: `Rfc2898DeriveBytes.Pbkdf2` algoritmasıyla 100.000 döngü ve rastgele 16 baytlık tuz (salt) ile güvenli parola saklama.
- **Doğrulama**: Giriş DTO'ları üzerinde `[Required]`, `[MinLength]`, `[EmailAddress]` gibi DataAnnotations ile güçlü model doğrulaması.
- **İlişkisel Tutarlılık**: EF Core `Restrict` silme kuralları ve doktor-hasta eşleştirmelerinde tekillik kısıtları (`IsUnique`).
- **CORS Uyumluluğu**: SignalR kimlik doğrulama gereksinimlerine uygun şekilde `SetIsOriginAllowed` + `AllowCredentials` yapılandırması.

---

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.
