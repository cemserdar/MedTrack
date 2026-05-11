using MedTrack.Infrastructure.Persistence;
using MedTrack.Domain.Interfaces;
using MedTrack.Infrastructure.Persistence.Repositories;
using MedTrack.Application.Interfaces;
using MedTrack.Application.Services;
using MedTrack.Application.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MedTrack API",
        Version = "v1",
        Description = "Medical tracking and appointment management system REST API",
        Contact = new OpenApiContact { Name = "MedTrack Support" }
    });

    // Add JWT Bearer authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Add JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MedTrack";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MedTrackUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MedTrack.Infrastructure")
    )
);

// Register all repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAllergyRepository, AllergyRepository>();
builder.Services.AddScoped<IChronicConditionRepository, ChronicConditionRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IDiagnosisCodeRepository, DiagnosisCodeRepository>();
builder.Services.AddScoped<IDoctorPatientRelationRepository, DoctorPatientRelationRepository>();
builder.Services.AddScoped<IImagingResultRepository, ImagingResultRepository>();
builder.Services.AddScoped<IImagingTypeRepository, ImagingTypeRepository>();
builder.Services.AddScoped<IInsuranceTypeRepository, InsuranceTypeRepository>();
builder.Services.AddScoped<ILabTestRepository, LabTestRepository>();
builder.Services.AddScoped<ILabTestTypeRepository, LabTestTypeRepository>();
builder.Services.AddScoped<IMedicalNoteRepository, MedicalNoteRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register application services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IMedicalNoteService, MedicalNoteService>();
builder.Services.AddScoped<ILabTestService, LabTestService>();
builder.Services.AddScoped<IImagingService, ImagingService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IReferenceDataService, ReferenceDataService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add SignalR
builder.Services.AddSignalR();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MedTrack API v1");
        options.RoutePrefix = string.Empty; // Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR hub
app.MapHub<MedTrack.WebAPI.Hubs.MedicalHub>("/hubs/medical");

app.Run();


