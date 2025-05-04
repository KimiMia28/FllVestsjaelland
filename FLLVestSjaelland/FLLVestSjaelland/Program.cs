//Imports
using FLLVestSjaelland.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using FLLVestSjaelland.Account;
using static System.Formats.Asn1.AsnWriter;
using FLLVestSjaelland.Data;
using Microsoft.EntityFrameworkCore;

//Der instandseres en builder til at lave en webapplikation
var builder = WebApplication.CreateBuilder(args);

// Tilføj services til builderen
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

//Der hentes connectionstring til databasen og databasekonteksten tilføjes til services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DBContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

//Webapplicationen bygges
var app = builder.Build();

// Konfigurer HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

//Opdaterer databasen med den nyeste migration (de nyeste ændringer)
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<DBContext>();
    context.Database.Migrate();
}

//Konfigurationer
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//Wepapplicationen startes
app.Run();