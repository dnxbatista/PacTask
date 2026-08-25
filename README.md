# PacTask

A full-stack task manager application currently under development. The backend API is available, and a React frontend is planned for a future release.

> **Current status:** Backend available · Frontend planned

## Stack

### Backend

- .NET 8
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- JWT authentication

### Frontend

- React
- Vite
- Planned

## Features

The API currently provides CRUD operations for:

- Users
- Environments
- Tasks

Authentication is implemented with JSON Web Tokens (JWT). Protected environment and task endpoints require a valid authenticated user token. Requests without a valid token will be rejected or will not return protected resources, depending on the endpoint behavior.

## Getting Started

### Prerequisites

Before running the backend, install:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server
- An IDE or code editor

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/dnxbatista/PacTask.git
   ```

2. Navigate to the repository directory:

   ```bash
   cd PacTask
   ```

3. Open `appsettings.Example.json` and update the configuration values, especially the database connection string and JWT settings.

4. Rename the configuration file:

   ```text
   appsettings.Example.json
   ```

   to:

   ```text
   appsettings.json
   ```

5. Navigate to the API project:

   ```bash
   cd PacTaskAPI/PacTaskAPI
   ```

6. Restore the project dependencies:

   ```bash
   dotnet restore
   ```

7. Start the application:

   ```bash
   dotnet watch run
   ```

The API should now be available at the local URL shown in the terminal.

## Configuration

Update the required values in `appsettings.json` before starting the application.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PacTaskDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "SigningKey": "your-development-secret-key",
    "Issuer": "PacTaskAPI",
    "Audience": "PacTaskClient",
  }
}
```

Use a long, unique secret key for local development and never commit real credentials, JWT signing keys, passwords, access tokens, or other sensitive values to the repository. Consider using environment variables or .NET User Secrets for local development.

## API Endpoints

The API includes the following resource groups:

| Resource | Operations | Authentication |
|---|---|---|
| Users | Create, read, update, delete | Depends on endpoint |
| Environments | Create, read, update, delete | JWT required |
| Tasks | Create, read, update, delete | JWT required |

For the complete endpoint list and request schemas, run the application and open the Swagger documentation, if enabled:

```text
/swagger
```

When using protected endpoints, authenticate through the login endpoint, copy the returned JWT, and provide it in the request header:

```text
Authorization: Bearer <your-jwt-token>
```

## Project Structure

```text
PacTask/
├── PacTaskAPI/
│   └── PacTaskAPI/
│       ├── Data/
│       ├── DTOs/
│       ├── Enums/
│       ├── Extensions/
│       ├── Interfaces/
│       ├── Mappers
│       ├── Migrations
│       ├── Models
│       ├── Properties
│       ├── Repositories
│       ├── Services
│       ├── appsettings.Example.json
│       ├── PacTaskAPI.csproj
│       ├── PacTaskAPI.http
│       └── Program.cs
└── README.md
```

## Roadmap

- [ ] Build the React and Vite frontend.
- [ ] Connect the frontend to the backend API.
- [ ] Add task filtering and sorting.
- [ ] Add task status management.
- [ ] Improve validation and error handling.
- [ ] Add automated tests.

## Current Limitations

- The frontend is not available yet.
- The API requires local configuration before it can run.
- Production deployment is not available yet.

## Contributing

This project is currently being developed as a portfolio project. Suggestions, feedback, and issue reports are welcome.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.