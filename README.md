# MedTrack - Medical Tracking and Appointment Management System

A comprehensive .NET 9 healthcare tracking application with real-time updates, JWT authentication, and containerized deployment.

## Features

- **Patient Management**: Create, read, update, and delete patient records
- **Appointment Scheduling**: Schedule and manage medical appointments
- **Doctor Management**: Manage doctor profiles and specialties
- **Medical Records**: Store medical notes, lab tests, imaging results, prescriptions
- **Real-time Updates**: SignalR integration for live notifications
- **JWT Authentication**: Secure API endpoints with JWT tokens
- **API Documentation**: Swagger/OpenAPI documentation
- **Docker Support**: Easy containerization with Docker and Docker Compose

## Architecture

- **Layered Architecture**: Domain → Infrastructure → Application → WebAPI
- **Entity Framework Core**: ORM for SQL Server database
- **AutoMapper**: Object-to-object mapping
- **CQRS Ready**: Services pattern for business logic
- **Repository Pattern**: Abstract data access layer

## Prerequisites

### Local Development
- .NET 9 SDK
- SQL Server 2019 or later
- Visual Studio 2022 or VS Code

### Docker Deployment
- Docker Desktop (includes Docker Compose)
- 4GB RAM minimum

## Quick Start

### Local Development

1. **Clone and Navigate**
```bash
git clone <repository-url>
cd MedTrack
```

2. **Update Connection String**
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=MedTrack;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. **Apply Database Migration**
```bash
dotnet ef database update --project MedTrack.Infrastructure
```

4. **Build Solution**
```bash
dotnet build
```

5. **Run WebAPI**
```bash
cd MedTrack.WebAPI
dotnet run
```

6. **Access Swagger UI**
Navigate to: `https://localhost:5001` or `http://localhost:5000`

### Docker Deployment

1. **Build Docker Image**
```bash
docker build -t medtrack:latest .
```

2. **Run with Docker Compose**
```bash
docker-compose up -d
```

3. **Access Services**
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health/status
- **SQL Server**: localhost:1433

4. **Stop Services**
```bash
docker-compose down
```

5. **View Logs**
```bash
docker-compose logs -f api
docker-compose logs -f sqlserver
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - Get JWT token
  ```json
  {
    "username": "user",
    "password": "password123"
  }
  ```
- `POST /api/auth/refresh` - Refresh token (requires valid token)

### Patients
- `GET /api/patients` - List all patients
- `GET /api/patients/{id}` - Get patient by ID
- `POST /api/patients` - Create patient
- `PUT /api/patients/{id}` - Update patient
- `DELETE /api/patients/{id}` - Delete patient
- `GET /api/patients/search/{name}` - Search patients by name

### Doctors
- `GET /api/doctors` - List all doctors
- `GET /api/doctors/{id}` - Get doctor by ID
- `POST /api/doctors` - Create doctor
- `PUT /api/doctors/{id}` - Update doctor
- `DELETE /api/doctors/{id}` - Delete doctor
- `GET /api/doctors/specialty/{specialty}` - Get doctors by specialty
- `GET /api/doctors/clinic/{clinicId}` - Get doctors by clinic

### Appointments
- `GET /api/appointments` - List all appointments
- `GET /api/appointments/{id}` - Get appointment by ID
- `POST /api/appointments` - Create appointment
- `PUT /api/appointments/{id}` - Update appointment
- `PATCH /api/appointments/{id}/status` - Update appointment status
- `DELETE /api/appointments/{id}` - Cancel appointment
- `GET /api/appointments/patient/{patientId}` - Get patient's appointments
- `GET /api/appointments/doctor/{doctorId}` - Get doctor's appointments
- `GET /api/appointments/date-range?start=...&end=...` - Get appointments by date range

### Medical Records
- **Prescriptions**: `/api/prescriptions` [GET, POST, PUT, DELETE]
- **Medical Notes**: `/api/medicalnotes` [GET, POST, PUT, DELETE]
- **Lab Tests**: `/api/labtests` [GET, POST, PUT, DELETE]
- **Imaging**: `/api/imaging` [GET, POST, PUT, DELETE]

### Reference Data
- `GET /api/referencedata/insurance-types`
- `GET /api/referencedata/lab-test-types`
- `GET /api/referencedata/imaging-types`
- `GET /api/referencedata/diagnosis-codes`

### Health Checks
- `GET /health/status` - Overall health
- `GET /health/live` - Liveness probe
- `GET /health/ready` - Readiness probe

## Real-time Updates (SignalR)

Connect to WebSocket hub at: `/hubs/medical`

Events:
- `AppointmentCreated`
- `AppointmentUpdated`
- `AppointmentCancelled`
- `MedicalNoteAdded`
- `PrescriptionCreated`
- `PrescriptionUpdated`
- `LabTestResultAvailable`
- `ImagingResultAvailable`
- `UserConnected`
- `UserDisconnected`

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Jwt": {
    "Secret": "change-to-secure-secret-key",
    "Issuer": "MedTrack",
    "Audience": "MedTrackUsers",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Database Schema

### Entities
- **Patients**: Patient records with demographics
- **Doctors**: Doctor profiles with specialties
- **Appointments**: Scheduling appointments between patients and doctors
- **Clinics**: Medical facilities
- **MedicalNotes**: Doctor's clinical notes
- **Prescriptions**: Medication prescriptions with items
- **LabTests**: Laboratory test records
- **ImagingResults**: Medical imaging examination results
- **Allergies**: Patient allergies
- **ChronicConditions**: Chronic health conditions
- **Users**: System users (future auth expansion)
- **Reference Data**: Insurance types, diagnosis codes, lab/imaging types

## Development Workflow

### Adding New Features

1. **Define Entity** in `MedTrack.Domain/Entities/`
2. **Create Repository Interface** in `MedTrack.Domain/Interfaces/`
3. **Implement Repository** in `MedTrack.Infrastructure/Persistence/Repositories/`
4. **Create DTO** in `MedTrack.Application/DTOs/`
5. **Add AutoMapper Mapping** in `MedTrack.Application/Mapping/MappingProfile.cs`
6. **Create Service Interface** in `MedTrack.Application/Interfaces/`
7. **Implement Service** in `MedTrack.Application/Services/`
8. **Create Controller** in `MedTrack.WebAPI/Controllers/`
9. **Register DI** in `MedTrack.WebAPI/Program.cs`
10. **Create Migration**: `dotnet ef migrations add FeatureName`

### Testing

Run tests (when available):
```bash
dotnet test
```

## Docker Troubleshooting

### SQL Server connection issues
```bash
# Check SQL Server logs
docker-compose logs sqlserver

# Test connection from API container
docker exec medtrack-api sqlcmd -S sqlserver,1433 -U sa -P Admin@123456 -Q "SELECT 1"
```

### API won't start
```bash
# View API logs
docker-compose logs api

# Rebuild without cache
docker-compose build --no-cache

# Remove volumes and restart
docker-compose down -v
docker-compose up -d
```

### Port conflicts
If ports 5000, 5001, or 1433 are in use:
- Edit `docker-compose.yml` to use different ports
- Or kill existing processes using those ports

## Security Considerations

1. **JWT Secret**: Change the JWT secret in production
2. **Database Password**: Change SQL Server password in `docker-compose.yml`
3. **HTTPS**: Enable HTTPS in production
4. **CORS**: Configure CORS policies for your frontend domain
5. **Rate Limiting**: Implement rate limiting for API endpoints
6. **Input Validation**: All endpoints validate input data

## Performance Optimization

- **Connection Pooling**: Configured in EF Core
- **Async/Await**: All database operations are async
- **Lazy Loading**: Disabled, use explicit includes
- **Query Optimization**: Use LINQ projections
- **Caching**: Can be added at service layer

## Future Enhancements

- [ ] Advanced audit logging
- [ ] File upload for medical documents
- [ ] SMS/Email notifications
- [ ] Mobile app (Xamarin/Flutter)
- [ ] Advanced reporting
- [ ] HIPAA compliance
- [ ] Two-factor authentication
- [ ] Role-based access control (RBAC)
- [ ] Multi-tenant support

## Support & Contributing

For issues and feature requests, please open an issue on the repository.

## License

This project is licensed under the MIT License - see LICENSE file for details.

## Contact

**Project**: MedTrack  
**Developed**: May 2026
