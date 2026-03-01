# Winter Sport Academy API

## Features
- **Clean Architecture**: Repository Pattern, Service Layer, and Controllers for separation of concerns.
- **Security**: RBAC (Role-Based Access Control) with JWT Bearer Tokens and ASP.NET Identity.
- **Database**: SQLite with Entity Framework Core Migrations.
- **Reliability**: Global Exception Handling and Business Logic validation for session conflicts.
- **Monitoring**: Built-in Health Checks for cloud-readiness.

## How to Run
1. Navigate to the project folder.
2. Ensure database is ready: `dotnet ef database update`
3. Run: `dotnet run`
4. Access:
   - **Swagger UI:** `http://localhost:5011/swagger/index.html`
   - **Health Status:** `http://localhost:5011/health`

## Docker Support
- **Build**: `docker build -t winter-sport-api .`
- **Run**: `docker run -d -p 5011:8080 winter-sport-api`
