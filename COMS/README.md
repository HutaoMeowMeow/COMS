# COMS - Canal Obstruction Monitoring System

## Project Overview

COMS is an AI and IoT-integrated mobile and web-based application designed to detect, monitor, and analyze canal obstructions in real time. It helps local government units, barangay officials, and community residents prevent flooding caused by blocked drainage systems.

## Technology Stack

| Component | Technology |
|-----------|-----------|
| **Backend Framework** | ASP.NET Core 10.0 |
| **Frontend** | Razor Pages + Bootstrap 5 + Chart.js |
| **Real-Time Communication** | SignalR |
| **Authentication** | JWT Bearer Tokens |
| **Database** | SQLite (via Entity Framework Core 10.0) |
| **Password Hashing** | BCrypt.Net |
| **API Documentation** | Swagger / OpenAPI |

## Database

- **Engine:** SQLite
- **File:** `coms.db` (auto-created in the project root)
- **ORM:** Entity Framework Core 10.0 with Code-First Migrations

### Entity Relationship Summary

| Entity | Description |
|--------|-------------|
| `User` | System users with roles: Admin, LGU, Barangay, Maintenance, Resident |
| `Canal` | Canal/drainage waterway metadata and thresholds |
| `Sensor` | IoT sensors deployed on canals (water level, flow rate, debris) |
| `SensorReading` | Time-series readings ingested from sensors |
| `ObstructionAlert` | Auto-generated alerts when thresholds are exceeded |
| `CommunityReport` | User-submitted obstruction reports with photos |
| `FloodRiskAssessment` | AI-generated risk scores and predictions |
| `Notification` | In-app notifications for stakeholders |

## Architecture

```
COMS/
â”œâ”€â”€ Models/           # Entity Framework entities
â”œâ”€â”€ DTOs/             # Request/response data contracts
â”œâ”€â”€ Data/             # DbContext and database initializer
â”œâ”€â”€ Services/         # Business logic layer
â”œâ”€â”€ Hubs/             # SignalR real-time hub
â”œâ”€â”€ Controllers/      # REST API controllers
â”œâ”€â”€ Pages/            # Razor Pages UI
â”‚   â”œâ”€â”€ Auth/         # Login and Register
â”‚   â”œâ”€â”€ Dashboard/    # Summary cards, charts, recent alerts
â”‚   â”œâ”€â”€ Canals/       # Canal list and detail views
â”‚   â”œâ”€â”€ Alerts/       # Alert management
â”‚   â”œâ”€â”€ Reports/      # Community reports
â”‚   â””â”€â”€ Risk/         # Flood risk analytics
â””â”€â”€ wwwroot/          # Static files (CSS, JS)
```

## Key Features

- **JWT Authentication & Role-Based Authorization**
- **IoT Sensor Data Ingestion** â€” accepts water level, flow rate, debris, turbidity, temperature
- **Threshold-Based Alerting** â€” automatic alerts when water levels exceed warning/critical thresholds
- **Real-Time Monitoring** â€” SignalR pushes new readings and alerts to connected clients
- **Community Reporting** â€” residents can submit obstruction reports
- **Flood Risk Analytics** â€” historical data aggregation with Chart.js visualizations
- **Responsive Web UI** â€” Bootstrap 5 dashboard for desktop and mobile

## Running the Application

```bash
cd COMS
dotnet run
```

The app starts on:
- **HTTP:** http://localhost:5287
- **HTTPS:** https://localhost:7275
- **Swagger UI:** https://localhost:7275/swagger
- **SignalR Hub:** ws://localhost:7275/hubs/monitoring

### Trust HTTPS Certificate (Windows)

```bash
dotnet dev-certs https --trust
```

## Default Seeded Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@coms.ph | Admin@123 |
| LGU | lgu@coms.ph | Lgu@123 |
| Barangay | barangay@coms.ph | Brgy@123 |
| Resident | resident@coms.ph | Res@123 |

## API Endpoints

| Controller | Endpoints |
|------------|-----------|
| `AuthController` | POST /api/Auth/register, POST /api/Auth/login, GET /api/Auth/me |
| `CanalsController` | GET/POST/PUT/DELETE /api/Canals, GET /api/Canals/status |
| `SensorsController` | GET/POST/PUT/DELETE /api/Sensors, GET /api/Sensors/canal/{id} |
| `ReadingsController` | POST /api/Readings/ingest, GET /api/Readings/sensor/{id} |
| `AlertsController` | GET/POST/PUT/DELETE /api/Alerts, GET /api/Alerts/active |
| `ReportsController` | GET/POST/PUT/DELETE /api/Reports |
| `RiskController` | GET /api/Risk, POST /api/Risk/canal/{id}/assess, GET /api/Risk/analytics |
| `NotificationsController` | GET /api/Notifications, PUT /api/Notifications/{id}/read |
| `DashboardController` | GET /api/Dashboard/summary, GET /api/Dashboard/canals-status, GET /api/Dashboard/analytics |

## Configuration

Edit `appsettings.json` to change:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=coms.db"
  },
  "Jwt": {
    "SecretKey": "COMS-SUPER-SECRET-KEY-CHANGE-ME-2024-VERY-LONG-RANDOM-KEY",
    "Issuer": "coms",
    "Audience": "coms-users",
    "ExpiryMinutes": "60"
  }
}
```

## Development

- Built with .NET 10.0 SDK
- Database is created and seeded automatically on first run
- No manual migration commands required (uses `EnsureCreatedAsync`)

## License

Academic project â€” Cebu City canal monitoring initiative.
