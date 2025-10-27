using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MudBlazor;
using MudBlazor.Services;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
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

builder.Services.AddSwaggerGen(options =>
{
    // swagger doc details
    options.SwaggerDoc("v1", new()
    {

        Title = "The BugBox API",
        Version = "v1",
        Description = """
                      <img src="/img/Logo Transparent white.png" />

                      This API is used by The Bug Tracker application when
                      executing in WebAssembly to interact with the backend.

                      If you think this project is interesting or would like to check out
                      the rest of my work, you can check out [my portfolio](https://cameronmills.netlify.app/)!

                      This API uses cookie-based authentication. To test the requests
                      below, you must first log in through the application to set a 
                      cookie in your browser and revieve a valid response from the 
                      "Test Request" buttons below.
                      """,
        Contact = new()
        {
            Name = "Cameron Mills",
            Email = "Cameronmills0713@gmail.com",
            Url = new("https://cameronmills.netlify.app/"),
        }

    });

    // show cookies as the suthentication scheme
    options.AddSecurityDefinition("cookie", new OpenApiSecurityScheme
    {
        Name = ".AspNetCore.Identity.Application",
        In = ParameterLocation.Cookie,
        Type = SecuritySchemeType.Http,
        Scheme = "cookie",
    });

    // show which endpoints require login
    options.OperationFilter<SecurityRequirementsOperationFilter>();

    // generate documentation for endpoints from XML comments
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

    // exclude docmentation for the built-in account endpoints
    options.DocInclusionPredicate((_, description) => 
        description.RelativePath is null || !description.RelativePath.StartsWith("Account"));
});


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddOutputCache();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies(cookieBuilder =>
    {
        cookieBuilder.ApplicationCookie!.Configure(config =>
        {
            config.Events.OnRedirectToLogin += (context) =>
            {
                if (context.Request.Path.StartsWithSegments("/api") || context.Request.HasJsonContentType())
                {
                    context.Response.StatusCode = 401;
                }

                return Task.CompletedTask;
            };

            config.Events.OnRedirectToAccessDenied += (context) =>
            {
                if (context.Request.Path.StartsWithSegments("/api") || context.Request.HasJsonContentType())
                {
                    context.Response.StatusCode = 403;
                }

                return Task.CompletedTask;
            };
        });
    });

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

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyDTOService, CompanyDTOService>();

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

app.UseSwagger(options => options.RouteTemplate = "/openapi/{documentName}.json");

app.MapScalarApiReference(options =>
{
    options.WithFavicon("/img/bugtrackerNew.png")
           .WithTitle("API Specification | BugBox")
           .WithTheme(ScalarTheme.BluePlanet);
}); // URL: /scalar/v1

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
