# Weather API — Full DevOps Project

![Build Status](https://github.com/manu07-oss/Weather-API/actions/workflows/ci-cd.yml/badge.svg)
![Docker Hub](https://img.shields.io/docker/pulls/manognavengala01/weather-api)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Docker](https://img.shields.io/badge/Docker-ready-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue)
![License](https://img.shields.io/badge/license-MIT-green)

A production-ready Weather API built with ASP.NET Core 8, containerized with Docker, monitored with Prometheus + Grafana, logged with Seq, and deployed via GitHub Actions CI/CD pipeline.

---

## What I Built — DevOps Engineer Perspective

This project demonstrates a complete DevOps lifecycle:
- Writing a backend API in C# with a database
- Packaging it into a Docker container using multi-stage builds
- Publishing the image to Docker Hub
- Automating build, test and push with GitHub Actions
- Running a full monitoring stack with Prometheus, Grafana and Seq
- Understanding Docker networking, reverse proxy, and service communication

---

## Architecture

```
User Browser
     │
     ▼
Nginx (port 80) ── reverse proxy
     ├──► React Frontend (port 3000)
     └──► C# Weather API (port 8085)
                │
                ▼
          PostgreSQL DB (port 5432)
                │
          ┌─────┴──────┐
          ▼            ▼
     Prometheus      Seq Logs
          │
          ▼
       Grafana
```

All services communicate over Docker's internal bridge network using service name resolution. No service exposes its internal port directly — Nginx acts as the single entry point.

---

## Tech Stack

| Layer | Technology | Why chosen |
|---|---|---|
| Frontend | React | Most popular UI library, component-based |
| Backend | ASP.NET Core 8 Web API | Enterprise grade, strongly typed C# |
| Database | PostgreSQL 16 | Industry standard open source relational DB |
| ORM | Entity Framework Core 8 | Maps C# classes to database tables |
| Containerization | Docker | Package once, run anywhere |
| Orchestration | Docker Compose | Run multiple containers with one command |
| CI/CD | GitHub Actions | Free, integrated with GitHub, industry standard |
| Registry | Docker Hub | Store and share Docker images publicly |
| Monitoring | Prometheus + Grafana | Industry standard metrics and dashboards |
| Logging | Seq | Structured log search and analysis |
| Reverse Proxy | Nginx | Route traffic, single entry point |
| Testing | xUnit + Moq + InMemory DB | Unit testing without real database |
| API Docs | Swagger / OpenAPI | Auto-generated interactive API documentation |

---

## Project Structure

```
Weather-API/
├── src/
│   └── WeatherApi/
│       ├── Controllers/
│       │   └── WeatherController.cs       # API endpoints
│       ├── Data/
│       │   └── WeatherContext.cs          # EF Core database context
│       ├── Models/
│       │   ├── WeatherForecast.cs         # API response model
│       │   └── WeatherSearch.cs           # Database table model
│       ├── Migrations/                    # EF Core auto-generated migrations
│       ├── Program.cs                     # App entry point and service registration
│       ├── appsettings.json               # Production configuration
│       └── appsettings.Development.json   # Local development configuration
├── tests/
│   └── WeatherApi.Tests/
│       └── WeatherControllerTests.cs      # 7 unit tests
├── monitoring/
│   └── prometheus.yml                     # Prometheus scrape configuration
├── .github/
│   └── workflows/
│       └── ci-cd.yml                      # GitHub Actions pipeline
├── Dockerfile                             # Multi-stage Docker build
├── docker-compose.yml                     # All services definition
├── .dockerignore                          # Files excluded from Docker image
└── README.md                              # This file
```

---

## API Endpoints

| Method | Endpoint | Description | Saves to DB |
|---|---|---|---|
| GET | `/api/weather` | Returns 5 random forecasts | No |
| GET | `/api/weather/{city}` | Returns forecast for any city | Yes |
| GET | `/api/weather/india` | Returns forecasts for 5 Indian cities | Yes |
| GET | `/api/weather/history` | Returns last 20 searches from DB | No |
| GET | `/health` | Health check including DB connectivity | No |
| GET | `/metrics` | Prometheus metrics endpoint | No |
| GET | `/swagger` | Interactive API documentation | No |

---

## Services and Ports

| Service | Internal Port | External Port | URL | Credentials |
|---|---|---|---|---|
| Weather API | 8080 | 8085 | http://localhost:8085 | none |
| Swagger UI | 8080 | 8085 | http://localhost:8085/swagger | none |
| Health Check | 8080 | 8085 | http://localhost:8085/health | none |
| Metrics | 8080 | 8085 | http://localhost:8085/metrics | none |
| PostgreSQL | 5432 | 5432 | localhost:5432 | postgres / postgres123 |
| Prometheus | 9090 | 9090 | http://localhost:9090 | none |
| Grafana | 3000 | 3000 | http://localhost:3000 | admin / admin123 |
| Seq Logs | 80 | 5341 | http://localhost:5341 | admin / admin123 |

---

## Prerequisites

| Tool | Version | Download |
|---|---|---|
| .NET SDK | 8.0 | https://dotnet.microsoft.com/download |
| Docker Desktop | Latest | https://www.docker.com/products/docker-desktop |
| Git | Latest | https://git-scm.com |
| Node.js | 18+ | https://nodejs.org (for frontend) |

---

## Quick Start

### Clone the repository
```bash
git clone https://github.com/manu07-oss/Weather-API.git
cd Weather-API
```

### Run with Docker Compose (recommended — runs everything)
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

### Run locally with .NET only
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/WeatherApi
```

---

## All Commands Used — Step by Step

### Project setup
```bash
# Create solution file
dotnet new sln -n WeatherApi

# Create Web API project
dotnet new webapi -n WeatherApi -o src/WeatherApi --no-openapi

# Create xUnit test project
dotnet new xunit -n WeatherApi.Tests -o tests/WeatherApi.Tests

# Add both projects to solution
dotnet sln add src/WeatherApi/WeatherApi.csproj
dotnet sln add tests/WeatherApi.Tests/WeatherApi.Tests.csproj

# Link test project to main project
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj reference src/WeatherApi/WeatherApi.csproj
```

### NuGet packages
```bash
# Swagger — interactive API documentation
dotnet add src/WeatherApi/WeatherApi.csproj package Swashbuckle.AspNetCore

# PostgreSQL with Entity Framework Core
dotnet add src/WeatherApi/WeatherApi.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.4
dotnet add src/WeatherApi/WeatherApi.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.4

# Health checks for database
dotnet add src/WeatherApi/WeatherApi.csproj package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --version 8.0.4

# Prometheus metrics
dotnet add src/WeatherApi/WeatherApi.csproj package prometheus-net.AspNetCore --version 8.2.1

# Test packages
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj package Moq
dotnet add tests/WeatherApi.Tests/WeatherApi.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory --version 8.0.4
```

### Build and test
```bash
dotnet restore       # Download all packages
dotnet build         # Compile the code
dotnet test          # Run all 7 unit tests
```

### Entity Framework migrations
```bash
# Install EF tools globally
dotnet tool install --global dotnet-ef

# Create initial migration (generates SQL from C# models)
dotnet ef migrations add InitialCreate --project src/WeatherApi

# Apply migration to database manually (optional — app does this on startup)
dotnet ef database update --project src/WeatherApi
```

### Docker commands
```bash
# Build image
docker build -t weather-api .

# Run container
docker run -d -p 8085:8080 --name weather-api weather-api

# View logs
docker logs weather-api

# View logs in real time
docker logs -f weather-api

# Check running containers
docker ps

# Stop container
docker stop weather-api

# Remove container
docker rm weather-api

# Force remove running container
docker rm -f weather-api

# Remove image
docker rmi weather-api

# Kill process using a port (run as Administrator)
taskkill /PID <PID> /F

# Find what is using a port
netstat -ano | findstr :8080

# Pull image from Docker Hub
docker pull manognavengala01/weather-api:latest
```

### Docker Compose commands
```bash
# Start all services
docker-compose up

# Start in background (detached)
docker-compose up -d

# Build and start
docker-compose up --build

# Stop all services
docker-compose down

# Stop and delete volumes
docker-compose down -v

# View all logs
docker-compose logs

# View specific service logs
docker-compose logs weatherapi
docker-compose logs db
docker-compose logs prometheus

# Follow logs in real time
docker-compose logs -f weatherapi

# Restart one service
docker-compose restart weatherapi

# Check status
docker-compose ps
```

### Git commands
```bash
git init
git add .
git commit -m "message"
git push origin main
git log --oneline -5
git checkout main
git status
```

---

## Docker Networking Explained

When `docker-compose up` runs, Docker creates a **private internal network**. All services join it automatically and can reach each other using their **service name** as hostname.

```
Inside Docker network — use service name:
  Host=db                    (PostgreSQL)
  http://weatherapi:8080     (API)
  http://prometheus:9090     (Prometheus)

Outside Docker (your browser) — use localhost + mapped port:
  http://localhost:8085      (API)
  http://localhost:9090      (Prometheus)
  http://localhost:3000      (Grafana)
```

### Port mapping
```yaml
ports:
  - "8085:8080"
#    ↑      ↑
#  laptop  container
```
The container always runs on 8080 internally. You choose what port to expose on your machine. Changing `8080:8080` to `8085:8080` fixed the Jenkins port conflict without changing anything inside the container.

---

## CI/CD Pipeline Explained

```
git push to main
      │
      ▼
GitHub Actions triggers
      │
      ▼
Job 1: Build and Test (ubuntu-latest)
  ├── actions/checkout@v4
  ├── actions/setup-dotnet@v4 (8.0.x)
  ├── dotnet restore
  ├── dotnet build --no-restore -c Release
  └── dotnet test (7 tests must pass)
      │
      ▼ (only runs if Job 1 passes)
Job 2: Docker Build and Push
  ├── docker/setup-buildx-action@v3
  ├── docker/login-action@v3 (uses GitHub secrets)
  ├── docker/metadata-action@v5 (generates tags)
  └── docker/build-push-action@v5
        ├── pushes: manognavengala01/weather-api:latest
        └── pushes: manognavengala01/weather-api:sha-xxxxxxx
```

### Why two tags?
- `latest` — always points to the most recent build
- `sha-xxxxxxx` — unique tag per commit, allows rollback to any version

### GitHub Secrets required
| Secret | Value |
|---|---|
| `DOCKERHUB_USERNAME` | manognavengala01 |
| `DOCKERHUB_TOKEN` | Docker Hub access token (not password) |

---

## Dockerfile Explained

```dockerfile
# Stage 1 — Build (uses full SDK image ~700MB)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src/WeatherApi/WeatherApi.csproj ./src/WeatherApi/
RUN dotnet restore                          # Cache layer — only re-runs if .csproj changes
COPY src/ ./src/
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2 — Runtime (uses tiny runtime image ~200MB)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN adduser --disabled-password appuser    # Security: never run as root
USER appuser
COPY --from=build /app/publish .           # Only copy published output, not build tools
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
HEALTHCHECK CMD wget -qO- http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "WeatherApi.dll"]
```

Multi-stage build means the final image only contains the runtime — no SDK, no source code, no build tools. This makes it smaller and more secure.

---

## Architecture Concepts

### Monolith vs Microservices

This project uses **Monolithic architecture** — one API handles everything.

| | Monolith (this project) | Microservices |
|---|---|---|
| Structure | One app, one database | Many small apps, each with own DB |
| Good for | Starting out, learning, small teams | Large teams, Netflix-scale systems |
| DevOps complexity | Simple — one pipeline | Complex — needs Kubernetes |
| Example | This Weather API | Netflix, Amazon, Uber |

### Cloud mapping

| Local (this project) | AWS | Azure |
|---|---|---|
| Docker container | ECS / EKS | Azure Container Apps |
| PostgreSQL | RDS | Azure Database for PostgreSQL |
| Docker Hub | ECR | Azure Container Registry |
| GitHub Actions | CodePipeline | Azure DevOps Pipelines |
| Prometheus + Grafana | CloudWatch | Azure Monitor |
| Nginx | ALB (Load Balancer) | Azure Application Gateway |

---

## Monitoring Setup

### Prometheus
- Scrapes `/metrics` every 15 seconds
- Stores time-series data
- Access: http://localhost:9090
- Check targets: http://localhost:9090/targets

### Grafana
1. Go to http://localhost:3000
2. Login: admin / admin123
3. Add data source → Prometheus → URL: `http://prometheus:9090`
4. Create dashboards to visualize request rate, error rate, latency

### Seq
1. Go to http://localhost:5341
2. Login: admin / admin123
3. Search and filter structured logs from the API

---

## Unit Tests

| Test | What it verifies |
|---|---|
| `Get_ReturnsExactlyFiveForecasts` | GET /api/weather returns 5 items |
| `GetByCity_WithValidCity_ReturnsForecast` | Valid city returns forecast |
| `GetByCity_WithEmptyCity_ReturnsBadRequest` | Empty city returns 400 |
| `GetIndia_ReturnsExactlyFiveCities` | India endpoint returns 5 cities |
| `GetIndia_AllTemperaturesAreRealistic` | India temps between 20-45°C |
| `GetByCity_SavesSearchToDatabase` | City search is saved to DB |
| (xUnit default) | Test infrastructure works |

Tests use **InMemory database** — no real PostgreSQL needed to run tests. This means the CI pipeline runs fast without spinning up a database container.

---

## Common Issues and Fixes

| Issue | Cause | Fix |
|---|---|---|
| `dotnet: command not found` | .NET not installed | Install .NET 8 SDK, restart terminal |
| `Dockerfile not found` | Wrong file extension | Rename `Dockerfile.yml` → `Dockerfile` |
| Port already in use | Another app on same port | Change external port in docker-compose |
| `Build FAILED` package version | Wrong .NET version package | Add `--version 8.0.4` to package install |
| Seq admin password error | New Seq requires password | Add `SEQ_FIRSTRUN_ADMINPASSWORD=admin123` |
| `Access is denied` killing process | Need admin rights | Run PowerShell as Administrator |
| Container unhealthy | `curl` not in base image | Use `wget` in HEALTHCHECK instead |

---

## What I Learned

- How to build a REST API with ASP.NET Core 8
- Multi-stage Docker builds to create small production images
- Docker Compose to orchestrate multiple services
- GitHub Actions CI/CD — automatic build, test, push on every commit
- Docker Hub image publishing and tagging strategy
- Docker networking — service name resolution, port mapping
- PostgreSQL with Entity Framework Core and auto-migrations
- Health checks that verify both app and database connectivity
- Prometheus metrics scraping and Grafana dashboards
- Structured logging with Seq
- Unit testing with in-memory database
- Environment-based configuration (Development vs Production)
- Security best practices — non-root containers, secrets management

---

## Interview Talking Points

**On the project:**
"I built a full stack Weather application with ASP.NET Core 8 backend and React frontend, connected via REST APIs. The app uses PostgreSQL for persistence with Entity Framework Core handling auto-migrations on startup."

**On Docker:**
"I used a multi-stage Dockerfile — the SDK image builds the app, the runtime image runs it. The final image is about 200MB instead of 700MB. I also run the container as a non-root user for security."

**On CI/CD:**
"Every push to main triggers a GitHub Actions pipeline with two jobs. Job 1 restores, builds and runs all tests. Only if tests pass does Job 2 build the Docker image and push it to Docker Hub with both a latest tag and a commit SHA tag for rollback capability."

**On networking:**
"All services communicate over Docker's internal bridge network using service name resolution. External traffic comes in through Nginx on port 80 which routes API calls to the backend and everything else to the React frontend."

**On monitoring:**
"Prometheus scrapes the /metrics endpoint every 15 seconds. Grafana connects to Prometheus to show dashboards for request rate, error rate and latency. Seq collects structured logs from the API for searchable log analysis."

---

## Author

**Vengala Manogna**
- GitHub: [@manu07-oss](https://github.com/manu07-oss)
- Docker Hub: [manognavengala01](https://hub.docker.com/u/manognavengala01)

---

## License

MIT License — feel free to use this project for learning and portfolio purposes.
