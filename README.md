# PacTask

PacTask is a full-stack task management application. Users can register and
log in, create environments, and manage tasks inside each environment.

## Technology stack

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server 2022
- JWT Bearer authentication
- Swagger/OpenAPI

### Frontend

- React 19
- TypeScript
- Vite
- Mantine UI
- Nginx (production container)

### Development and deployment

- Docker and Docker Compose
- Node.js 20 for frontend development
- .NET 8 SDK for backend development

## Repository structure

```text
PacTask/
├── PacTaskAPI/
│   ├── Dockerfile
│   ├── PacTaskAPI/
│   │   ├── Controllers/
│   │   ├── Data/
│   │   ├── Migrations/
│   │   ├── Models/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   ├── Program.cs
│   │   └── PacTaskAPI.csproj
│   └── PacTaskAPI.slnx
├── PacTaskWEB/
│   ├── Dockerfile
│   ├── src/
│   ├── package.json
│   └── vite.config.ts
├── .env.example
├── docker-compose.yaml
└── README.md
```

## Running with Docker Compose

### Prerequisites

Install and start:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Docker Compose (included with Docker Desktop)

Docker Desktop must be running with the Linux container engine enabled.

### Setup

1. Clone the repository and enter its directory:

   ```powershell
   git clone https://github.com/dnxbatista/PacTask.git
   cd PacTask
   ```

2. Create the environment file:

   ```powershell
   Copy-Item .env.example .env
   ```

3. Review `.env` and set a unique `SA_PASSWORD` and `JWT_SIGNING_KEY`.
   `JWT_SIGNING_KEY` must be at least 64 characters because the API signs
   tokens with HS512.

4. Build and start all services:

   ```powershell
   docker compose up --build
   ```

   To run in the background:

   ```powershell
   docker compose up --build -d
   ```

The first startup can take a few minutes while the SQL Server image is
downloaded and initialized. The API automatically applies pending Entity
Framework Core migrations when it starts.

### Docker service URLs

| Service | URL |
|---|---|
| Web application | <http://localhost:5173> |
| API | <http://localhost:8080> |
| Swagger UI | <http://localhost:8080/swagger> |
| SQL Server | `localhost:1433` |

Useful commands:

```powershell
# Show service status
docker compose ps

# Follow service logs
docker compose logs -f api

# Stop services
docker compose down

# Stop services and remove the database volume, if one is added later
docker compose down -v
```

The frontend is compiled into the web image, so `VITE_API_URL` is supplied as
a Docker build argument. The browser should use `http://localhost:8080`; the
API container itself connects to SQL Server using the service name `db`.

## Running locally without Docker

### Backend

Prerequisites: .NET 8 SDK and a reachable SQL Server instance.

1. Enter the API project directory:

   ```powershell
   cd PacTaskAPI\PacTaskAPI
   ```

2. Configure the connection string and JWT settings in
   `appsettings.Development.json` or through environment variables.

3. Restore dependencies and run:

   ```powershell
   dotnet restore
   dotnet run
   ```

The API applies pending migrations at startup. The URL is printed in the
terminal; Swagger is available at `/swagger` when running in Development.

### Frontend

Prerequisites: Node.js 20 or newer.

1. Enter the frontend directory:

   ```powershell
   cd PacTaskWEB
   ```

2. Install dependencies:

   ```powershell
   npm ci
   ```

3. Start the Vite development server:

   ```powershell
   npm run dev
   ```

To create a production frontend build:

```powershell
npm run build
```

Set `VITE_API_URL` in `PacTaskWEB/.env` when the API is not running at
`http://localhost:8080`.

## Configuration

Docker Compose reads the root `.env` file. Use `.env.example` as the template:

| Variable | Purpose | Docker example |
|---|---|---|
| `SA_PASSWORD` | SQL Server `sa` password | A strong password |
| `DB_NAME` | Application database name | `PacTaskDb` |
| `JWT_ISSUER` | JWT issuer claim | `http://localhost:8080` |
| `JWT_AUDIENCE` | JWT audience claim | `http://localhost:8080` |
| `JWT_SIGNING_KEY` | JWT HS512 signing key | A random key of 64+ characters |
| `VITE_API_URL` | API URL used by the browser | `http://localhost:8080` |

Do not commit `.env` or real credentials. Use `.env.example` for shareable
configuration documentation.

## API overview

The main endpoint groups are:

| Resource | Operations | Authentication |
|---|---|---|
| Users | Register and login | Depends on endpoint |
| Environments | Create, list, update, delete | JWT required |
| Tasks | Create, list, update, delete | JWT required |

Use Swagger UI to see the complete request and response schemas. After login,
copy the returned token and authorize requests with:

```text
Authorization: Bearer <token>
```

## Database migrations

Migrations are stored in `PacTaskAPI/PacTaskAPI/Migrations`. The API runs
`Database.MigrateAsync()` during startup, creating `PacTaskDb` when necessary
and applying pending migrations.

If the database container was initialized with incorrect credentials, remove
the existing SQL Server data and start again:

```powershell
docker compose down -v
docker compose up --build
```

Only use `down -v` when it is safe to delete the local database data.

## Troubleshooting

- **Docker API HTTP 500 or named-pipe errors:** start or restart Docker
  Desktop and confirm that the Linux engine is running.
- **`COPY ... not found` during a build:** run Compose from the repository root
  so the configured build contexts are used.
- **`node.exe not found` in the web build:** remove host dependencies from the
  image by rebuilding with `docker compose build --no-cache web`; the web
  `.dockerignore` also prevents Windows `node_modules` from being copied.
- **SQL error 4060 / database login failure:** confirm the `SA_PASSWORD` is
  consistent and recreate the local database with `docker compose down -v`.
- **JWT key-size error:** use a `JWT_SIGNING_KEY` with at least 64 characters.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).
