# PacTask API Documentation

This folder is a lightweight API contract and implementation map for the PacTask backend.

## Overview

The API is a .NET ASP.NET Core Web API for managing:
- users
- task environments
- tasks inside each environment

The system uses JWT bearer authentication and stores data in SQL Server via Entity Framework Core.

## Base URL

Local development:
- HTTPS: https://localhost:7169
- HTTP: http://localhost:5021

## Auth model

Authentication is handled with JWT bearer tokens.

Headers required for protected endpoints:

```http
Authorization: Bearer <token>
```

The token is returned by:
- POST /api/User/register
- POST /api/User

The token claims include:
- email
- username

## Main resources

- User: account registration, login, and profile update
- Environment: user-owned task groupings
- Task: tasks belonging to an environment

## Relationship model

- User has many Environments
- Environment belongs to one User
- Environment has many Tasks
- Task belongs to one Environment

## Status enum

```json
{
  "NotDone": 0,
  "Done": 1
}
```

## Important behavior notes

- A user can only access environments they own.
- A task can only be modified by the user who owns the parent environment.
- Environment and task routes are protected with `[Authorize]`.
- User registration and login are public endpoints.
- The API returns plain text error messages for validation and authorization failures.

## Files in this repo relevant to the API

- Controllers/
  - UserController.cs
  - EnvironmentController.cs
  - TaskController.cs
- Models/
  - UserEntity.cs
  - EnvironmentEntity.cs
  - TaskEntity.cs
- DTOs/
  - User/*.cs
  - Environment/*.cs
  - Task/*.cs
- Data/ApplicationDBContext.cs
- Services/TokenService.cs
- Services/PasswordService.cs

## Endpoint summary

- POST /api/User/register
- POST /api/User
- PUT /api/User
- GET /api/Environment
- POST /api/Environment
- PUT /api/Environment/{id}
- DELETE /api/Environment/{id}
- GET /api/Task/{id}
- GET /api/Task/{id}/unique
- POST /api/Task/{id}
- PUT /api/Task/{id}
- DELETE /api/Task/{id}

For detailed route contracts, request bodies, and response samples, see:
- endpoints.md
- models.md

For a machine-readable contract, see the project root openapi.yaml.
