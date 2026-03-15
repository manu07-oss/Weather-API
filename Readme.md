# Weather API — Full DevOps Project

![Build Status](https://github.com/manu07-oss/Weather-API/actions/workflows/ci-cd.yml/badge.svg)
![Docker Hub](https://img.shields.io/docker/pulls/manognavengala01/weather-api)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Docker](https://img.shields.io/badge/Docker-ready-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue)

A production-ready Weather API built with ASP.NET Core 8, containerized with Docker, monitored with Prometheus + Grafana, logged with Seq, and deployed via GitHub Actions CI/CD pipeline.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 Web API |
| Database | PostgreSQL 16 + Entity Framework Core |
| Containerization | Docker + Docker Compose |
| CI/CD | GitHub Actions |
| Registry | Docker Hub |
| Monitoring | Prometheus + Grafana |
| Logging | Seq |
| Testing | xUnit + Moq + InMemory DB |

---

## Project Structure

```
Weather-API/
├── src/
│   └── WeatherApi/
│       ├── Controllers/
│       │   └── WeatherController.cs     # API endpoints
│       ├── Data/
│       │   └── WeatherContext.cs        # Database context
│       ├── Models/
│       │   ├── WeatherForecast.cs       # Response model
│       │   └── WeatherSearch.cs        # Database model
│       ├── Migrations/                  # EF Core migrations
│       ├── Program.cs                   # App entry point
│       ├── appsettings.json             # Production config
│       └── appsettings.Development.json # Local config
├── tests/
│   └── WeatherApi.Tests/
│       └── WeatherControllerTests.cs   # 7 unit tests
├── monitoring/
│   └── prometheus.yml                  # Prometheus scrape config
├── .github/
│   └── workflows/
│       └── ci-cd.yml                   # GitHub Actions pipeline
├── Dockerfile                          # Multi-stage Docker build
├── docker-compose.yml                  # All 5 services
├── .dockerignore                       # Docker ignore rules
└── README.md                           # This file
```

---

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/weather` | Returns 5 weather forecasts |
| GET | `/api/weather/{city}` | Returns forecast for a city |
| GET | `/api/weather/india` | Returns forecasts for 5 Indian cities |
| GET | `/api/weather/history` | Returns last 20 searches from DB |
| GET | `/health` | Health check (includes DB check) |
| GET | `/metrics` | Prometheus metrics endpoint |
| GET | `/swagger` | Interactive API documentation |

---

## Prerequisites

| Tool | Version | Download |
|---|---|---|
| .NET SDK | 8.0 | https://dotnet.microsoft.com/download |
| Docker Desktop | Latest | https://www.docker.com/products/docker-desktop |
| Git | Latest | https://git-scm.com |

---

## Quick Start

### Clone the repository
```bash
git clone https://github.com/manu07-oss/Weather-API.git
cd Weather-API
```

### Run locally with .NET
```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Run the app (needs PostgreSQL running locally)
dotnet run --project src/WeatherApi
```

### Run with Docker Compose (recommended)
```bash
# Build and start all services
docker-compose up --build

# Run in background
docker-compose up --build -d

# Stop all services
docker-compose down

# Stop and remove volumes (wipes database)
docker-compose down -v
```

---

## Services & Ports

| Service | URL | Credentials |
|---|---|---|
| Weather API | http://localhost:8085 | none |
| Swagger UI | http://localhost:8085/swagger | none |
| Health Check | http://localhost:8085/health | none |
| Metrics | http://localhost:8085/metrics | none |
| PostgreSQL | localhost:5432 | postgres / postgres123 |
| Prometheus | http://localhost:9090 | none |
| Grafana | http://localhost:3000 | admin / admin123 |
| Seq Logs | http://localhost:5341 | admin / admin123 |

---

## Docker Commands

```bash
# Build the image manually
docker build -t weather-api .

# Run the container
docker run -d -p 8085:8080 --name weather-api weather-api

# View container logs
docker logs weather-api

# View logs in real time
docker logs -f weather-api

# Check container health
docker ps

# Stop container
docker stop weather-api

# Remove container
docker rm weather-api

# Remove image
docker rmi weather-api

# Pull from Docker Hub
docker pull manognavengala01/weather-api:latest
```

---

## Docker Compose Commands

```bash
# Start all services
docker-compose up

# Start in background
docker-compose up -d

# Build and start
docker-compose up --build

# Stop all services
docker-compose down

# View logs of all services
docker-compose logs

# View logs of specific service
docker-compose logs weatherapi
docker-compose logs db
docker-compose logs prometheus

# View logs in real time
docker-compose logs -f weatherapi

# Restart a specific service
docker-compose restart weatherapi

# Check status of all services
docker-compose ps
```

---

## .NET Commands Used

```bash
# Create solution
dotnet new sln -n WeatherApi

# Create Web API project
dotnet new webapi -n WeatherApi -o src/WeatherApi --no-openapi

# Create test project
dotnet new xunit -n WeatherApi.Tests -o tests/WeatherApi.Tests

# Add projects to solution
dotnet sln add src/WeatherApi/WeatherApi.csproj
dotnet sln add tests/WeatherApi.Tests/WeatherApi.Tests.csproj

# Add NuGet packages
dotnet add src/WeatherApi/WeatherApi.csproj package Swashbuckle.AspNetCore
dotnet add src/WeatherApi/WeatherApi.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.4
dotnet add src/WeatherApi/WeatherApi.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.4
dotnet add src/WeatherApi/WeatherApi.csproj package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --version 8.0.4
dotnet add src/WeatherApi/WeatherApi.csproj package prometheus-net.AspNetCore --version 8.2.1
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj package Moq
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory --version 8.0.4

# Add project reference
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj reference src/WeatherApi/WeatherApi.csproj

# Restore, build, test
dotnet restore
dotnet build
dotnet test

# Entity Framework migrations
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/WeatherApi
dotnet ef database update --project src/WeatherApi
```

---

## GitHub Actions Pipeline

The CI/CD pipeline runs automatically on every push to `main`:

```
Push to main
    │
    ▼
Job 1: Build & Test
    ├── dotnet restore
    ├── dotnet build
    └── dotnet test (7 tests)
    │
    ▼ (only if tests pass)
Job 2: Docker Build & Push
    ├── docker login (using secrets)
    ├── docker build
    └── docker push → manognavengala01/weather-api:latest
                   → manognavengala01/weather-api:sha-xxxxxxx
```

### GitHub Secrets Required

| Secret | Description |
|---|---|
| `DOCKERHUB_USERNAME` | Docker Hub username |
| `DOCKERHUB_TOKEN` | Docker Hub access token |

---

## Monitoring

### Prometheus
- Scrapes `/metrics` every 15 seconds
- Access at http://localhost:9090
- Check targets: http://localhost:9090/targets

### Grafana
- Connect to Prometheus: Settings → Data Sources → Add Prometheus → `http://prometheus:9090`
- Access at http://localhost:3000 (admin/admin123)

### Seq
- Structured log search UI
- Access at http://localhost:5341 (admin/admin123)

---

## Environment Variables

| Variable | Description | Default |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | App environment | Production |
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection | see docker-compose |
| `POSTGRES_DB` | Database name | weatherdb |
| `POSTGRES_USER` | DB username | postgres |
| `POSTGRES_PASSWORD` | DB password | postgres123 |

---

## Author

**Vengala Manogna**
- GitHub: [@manu07-oss](https://github.com/manu07-oss)
- Docker Hub: [manognavengala01](https://hub.docker.com/u/manognavengala01)

---

## License

MIT License — feel free to use this project for learning and portfolio purposes.
