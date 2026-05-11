# Multi-stage build
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["MedTrackSolution.sln", "."]
COPY ["MedTrack.Domain/MedTrack.Domain.csproj", "MedTrack.Domain/"]
COPY ["MedTrack.Infrastructure/MedTrack.Infrastructure.csproj", "MedTrack.Infrastructure/"]
COPY ["MedTrack.Application/MedTrack.Application.csproj", "MedTrack.Application/"]
COPY ["MedTrack.WebAPI/MedTrack.WebAPI.csproj", "MedTrack.WebAPI/"]

# Restore dependencies
RUN dotnet restore "MedTrackSolution.sln"

# Copy source code
COPY ["MedTrack.Domain/", "MedTrack.Domain/"]
COPY ["MedTrack.Infrastructure/", "MedTrack.Infrastructure/"]
COPY ["MedTrack.Application/", "MedTrack.Application/"]
COPY ["MedTrack.WebAPI/", "MedTrack.WebAPI/"]

# Build and publish
RUN dotnet build "MedTrackSolution.sln" -c Release -o /app/build
RUN dotnet publish "MedTrack.WebAPI/MedTrack.WebAPI.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install SQL Server tools for migrations (optional but useful)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app from build stage
COPY --from=build /app/publish .

# Create non-root user for security
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5000/health/status || exit 1

# Expose ports
EXPOSE 80 443

# Environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Set entrypoint
ENTRYPOINT ["dotnet", "MedTrack.WebAPI.dll"]
