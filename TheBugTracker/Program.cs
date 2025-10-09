using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TheBugTracker.Client.Services.Interfaces;
using TheBugTracker.Components;
using TheBugTracker.Components.Account;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services;
using TheBugTracker.Services.Interfaces;
using TheBugTracker.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddOutputCache();
builder.Services.AddMudServices();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = DataUtility.GetConnectionString(builder.Configuration) 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql
    (
        connectionString,
        options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
     ));



builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()       //Custom Claims
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectDTOService, ProjectDTOService>();


var app = builder.Build();


// Run any Migrations
var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TheBugTracker.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseOutputCache();
app.MapControllers();



app.Run();
