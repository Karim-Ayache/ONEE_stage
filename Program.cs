using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Localization Services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container with View & DataAnnotations localization enabled
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Register ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Register Custom Application Services (Dependency Injection)
builder.Services.AddScoped<IOffersService, OffersService>();
builder.Services.AddScoped<IApplicationsService, ApplicationsService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

var app = builder.Build();

// Seed Default System Administrator on Startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure database is created
    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        context.Users.Add(new ONEE_Stage.Models.Entities.User
        {
            Email = "admin@onee.ma",
            FirstName = "System",
            LastName = "Admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@2026!"),
            Role = ONEE_Stage.Models.Enums.UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 2. Configure Request Localization Middleware (Default FR, support EN)
var supportedCultures = new[]
{
    new CultureInfo("fr"),
    new CultureInfo("en")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    }
});

app.UseRouting();

// Authentication MUST be placed before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Default Route Map set to Offers/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Offers}/{action=Index}/{id?}");

app.Run();