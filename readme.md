# SpaCleanTemplate

## Resources

https://www.youtube.com/watch?v=vRZ8ucGac8M



https://github.dev/ardalis/Specification/blob/main/Specification.EntityFrameworkCore/src/Ardalis.Specification.EntityFrameworkCore/RepositoryBaseOfT.cs


https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0

https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0

## Install dotnet-ef
- `dotnet tool install --global dotnet-ef`

## Add the first migration
- `dotnet ef migrations add InitialMigration --context AppDbContext --output-dir Data/Migrations --project ../NetApiCleanTemplate.Infrastructure`
- `dotnet ef migrations add InitialMigration --context AppIdentityDbContext --output-dir Identity/Migrations --project ../NetApiCleanTemplate.Infrastructure`

## Add migration
- `dotnet ef migrations add InitialMigration --context AppDbContext --project ../NetApiCleanTemplate.Infrastructure`
- `dotnet ef migrations add AddedTenants --context AppIdentityDbContext --project ../NetApiCleanTemplate.Infrastructure`

## Update database
- `dotnet ef database update --context AppDbContext`

## Generate an update script for production
- `dotnet ef migrations script --idempotent --context AppDbContext --output update.sql`








