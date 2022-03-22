# dotnet ef migrations add Init -c IdentityContext -o IdentityMigrations
dotnet ef database update  -c IdentityContext 