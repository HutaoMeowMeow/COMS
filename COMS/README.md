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
<<<<<<< HEAD
| **Database** | Firebase Firestore |
=======
| **Database** | SQLite (via Entity Framework Core 10.0) |
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
| **Password Hashing** | BCrypt.Net |
| **API Documentation** | Swagger / OpenAPI |

## Database

<<<<<<< HEAD
- **Engine:** Firebase Firestore
- **Collections:** `users`, `canals`, `sensors`, `sensor_readings`, `obstruction_alerts`, `community_reports`, `flood_risk_assessments`, `notifications`, `announcements`
- **Authentication:** Service account via `firebase-service-account.json`

### Entity Summary
=======
- **Engine:** SQLite
- **File:** `coms.db` (auto-created in the project root)
- **ORM:** Entity Framework Core 10.0 with Code-First Migrations

### Entity Relationship Summary
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

| Entity | Description |
|--------|-------------|
| `User` | System users with roles: Admin, LGU, Barangay, Maintenance, Resident |
| `Canal` | Canal/drainage waterway metadata and thresholds |
| `Sensor` | IoT sensors deployed on canals (water level, flow rate, debris) |
| `SensorReading` | Time-series readings ingested from sensors |
| `ObstructionAlert` | Auto-generated alerts when thresholds are exceeded |
<<<<<<< HEAD
| `CommunityReport` | User-submitted obstruction reports with photos and completion tracking |
| `FloodRiskAssessment` | AI-generated risk scores and predictions |
| `Notification` | In-app notifications for stakeholders |
| `Announcement` | LGU/Barangay community updates and announcements |
=======
| `CommunityReport` | User-submitted obstruction reports with photos |
| `FloodRiskAssessment` | AI-generated risk scores and predictions |
| `Notification` | In-app notifications for stakeholders |
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

## Architecture

```
COMS/
<<<<<<< HEAD
├── Models/           # Data models
├── DTOs/             # Request/response data contracts
├── Data/             # Firestore repository and initializer
=======
├── Models/           # Entity Framework entities
├── DTOs/             # Request/response data contracts
├── Data/             # DbContext and database initializer
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
├── Services/         # Business logic layer
├── Hubs/             # SignalR real-time hub
├── Controllers/      # REST API controllers
├── Pages/            # Razor Pages UI
│   ├── Auth/         # Login and Register
│   ├── Dashboard/    # Summary cards, charts, recent alerts
│   ├── Canals/       # Canal list and detail views
│   ├── Alerts/       # Alert management
│   ├── Reports/      # Community reports
│   └── Risk/         # Flood risk analytics
└── wwwroot/          # Static files (CSS, JS)
```

## Key Features

- **JWT Authentication & Role-Based Authorization**
- **IoT Sensor Data Ingestion** — accepts water level, flow rate, debris, turbidity, temperature
- **Threshold-Based Alerting** — automatic alerts when water levels exceed warning/critical thresholds
- **Real-Time Monitoring** — SignalR pushes new readings and alerts to connected clients
<<<<<<< HEAD
- **Community Reporting** — residents submit obstruction reports with photos
- **Task Completion Workflow** — LGU/Barangay mark reports complete with images and remarks
- **Announcements** — LGU/Barangay post community updates visible to all residents
- **Resident Data Isolation** — each resident sees only their own reports
- **Flood Risk Analytics** — historical data aggregation with Chart.js visualizations
- **Responsive Web UI** — Bootstrap 5 dashboard for desktop and mobile

## Role-Based Access

| Role | Permissions |
|------|-------------|
| **Admin** | Full system access |
| **LGU** | Manage canals, sensors, alerts, reports; mark reports complete; create announcements |
| **Barangay** | Manage reports and announcements; verify and complete tasks |
| **Maintenance** | Manage sensors and readings |
| **Resident** | Submit reports, view own reports, view completed reports, view announcements |

=======
- **Community Reporting** — residents can submit obstruction reports
- **Flood Risk Analytics** — historical data aggregation with Chart.js visualizations
- **Responsive Web UI** — Bootstrap 5 dashboard for desktop and mobile

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
## Firebase Setup

1. Place your `firebase-service-account.json` in the project root
2. Ensure Firestore is enabled in your Firebase project
3. Deploy Firestore indexes:

```bash
firebase deploy --only firestore:indexes
```

=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
<<<<<<< HEAD
| `ReportsController` | GET/POST/PUT/DELETE /api/Reports, GET /api/Reports/my, GET /api/Reports/completed, POST /api/Reports/{id}/complete |
| `AnnouncementsController` | GET/POST/PUT/DELETE /api/Announcements |
=======
| `ReportsController` | GET/POST/PUT/DELETE /api/Reports |
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
| `RiskController` | GET /api/Risk, POST /api/Risk/canal/{id}/assess, GET /api/Risk/analytics |
| `NotificationsController` | GET /api/Notifications, PUT /api/Notifications/{id}/read |
| `DashboardController` | GET /api/Dashboard/summary, GET /api/Dashboard/canals-status, GET /api/Dashboard/analytics |

## Configuration

Edit `appsettings.json` to change:

```json
{
<<<<<<< HEAD
  "Firebase": {
    "ProjectId": "coms-eldnet",
    "ServiceAccountPath": "firebase-service-account.json"
=======
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=coms.db"
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
<<<<<<< HEAD
- Data is stored in Firebase Firestore
- Database is seeded automatically on first run
- Ensure `firebase-service-account.json` is present in the project root
=======
- Database is created and seeded automatically on first run
- No manual migration commands required (uses `EnsureCreatedAsync`)
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

## License

Academic project — Cebu City canal monitoring initiative.
