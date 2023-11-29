# Erstellung des Projektes

das Projekt wurde durch [dotnet scaffolding](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-7.0&tabs=netcore-cli#scaffold-identity-into-a-razor-project-with-authorization-1) erstellt :

    dotnet new webapp -au Individual -o DemoSalestoolLogin
    dotnet tool install -g dotnet-aspnet-codegenerator
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    dotnet add package Microsoft.AspNetCore.Identity.UI
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet aspnet-codegenerator identity -dc DemoSalestoolLogin.Data.ApplicationDbContext

nachträglich wurde eine zusätzliche dependency hinzugefügt `dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect`
und eine kleine Anpassung in der `Program.cs` und der `ExternalLoginModel.cshtml.cs` vorgenommen.

Alles wichtige passiert dann in der `ExternalLoginModel.cshtml.cs` die man nach belieben anpassen kann.

Nun kann man sich über https://localhost:5051/Identity/Account/ExternalLogin?returnUrl=%2F&provider=asvg beim salestool anmelden.