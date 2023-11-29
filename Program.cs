using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DemoSalestoolLogin.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

/* start: code added by envisia GmbH */
builder.Services.AddAuthentication().AddOpenIdConnect("asvg", options =>
{
    options.Authority = "https://salestool.asvg-solutions.de/";
    options.MetadataAddress = "https://salestool.asvg-solutions.de/.well-known/openid-configuration";
    options.ClientId = "Weyrauch";
    options.Scope.Add("profile");
    options.Scope.Add("openid");

    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;

    options.GetClaimsFromUserInfoEndpoint = false;
    options.ResponseType = OpenIdConnectResponseType.Code;

    options.Events = new OpenIdConnectEvents
    {
        OnRemoteFailure = ctx =>
        {
            // if somebody bookmarks the asvg salestool login portal a correlation failure would happen
            // this fixes it by restarting the login flow
            if (ctx.Failure?.Message == "Correlation failed.")
            {
                ctx.Response.Redirect("/Identity/Account/ExternalLogin?returnUrl=%2F&provider=asvg");
                ctx.HandleResponse();
            }

            return Task.CompletedTask;
        },
    };
});
/* end: code added by envisia GmbH */

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
