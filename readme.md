# SpaCleanTemplate

## Resources

https://www.youtube.com/watch?v=vRZ8ucGac8M



https://github.dev/ardalis/Specification/blob/main/Specification.EntityFrameworkCore/src/Ardalis.Specification.EntityFrameworkCore/RepositoryBaseOfT.cs


https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0

https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0

## Install dotnet-ef
- `dotnet tool install --global dotnet-ef`

## Add the first migration
- `dotnet ef migrations add InitialMigration --context AppDbContext --output-dir Data/Migrations --project ../SpaCleanTemplate.Infrastructure`
- `dotnet ef migrations add InitialMigration --context AppIdentityDbContext --output-dir Identity/Migrations --project ../SpaCleanTemplate.Infrastructure`

## Add migration
- `dotnet ef migrations add InitialMigration --context AppDbContext`

## Update database
- `dotnet ef database update --context AppDbContext`

## Generate an update script for production
- `dotnet ef migrations script --idempotent --context AppDbContext --output update.sql`








