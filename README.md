# MapApp Backend (Earlier Split Version)

An earlier separately stored backend version of the MapApp geospatial platform.

## Overview

An earlier separately stored backend version of the MapApp geospatial platform. The description and capabilities in this document are limited to behavior that can be verified in the repository source.

## Key Features

- Point and area API modules
- PostGIS geometry persistence
- Authentication infrastructure
- Application and infrastructure project separation

## Tech Stack

- C#
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- PostGIS
- NetTopologySuite
- MediatR
- JWT

## Architecture

Domain, Application, Infrastructure, and API projects separate geometry entities, handlers, persistence services, and HTTP controllers.

## Project Structure

- `MapApp.Domain/` — entities
- `MapApp.Application/` — features and DTOs
- `MapApp.Infrastructure/` — persistence, migrations, and services
- `MapApp.API/` — controllers and middleware

## Getting Started

Run the commands appropriate to the project root:

```bash
dotnet restore MapApp.sln
dotnet run --project MapApp.API
```

## Environment Variables

Configure these names through local environment/configuration files. Do not commit secret values.

```env
ConnectionStrings__DefaultConnection=
Jwt__Key=
```

## Technical Highlights

- PostGIS-backed geometry data
- Layered ASP.NET Core solution
- MediatR handlers

## Possible Improvements

- Add or expand automated tests around core workflows.
- Document deployment and environment-specific configuration.
- Add CI checks for build, linting, and tests where they are not already present.

## Verification Notes

This repository overlaps with the newer combined MapApp repository and should not be presented as a separate CV project.
