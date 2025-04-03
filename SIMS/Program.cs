using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SIMS.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Cấu hình DbContext từ appsettings.json
builder.Services.AddDbContext<SIMSDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("SIMS_DB")));

// Cấu hình MVC + JSON + View Compilation
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    .AddRazorRuntimeCompilation();

// Cấu hình Session & HTTP Context
builder.Services.AddSession(options =>
{
    // Customize session options here if needed, like timeout duration
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Set session timeout to 30 minutes
});
builder.Services.AddHttpContextAccessor();

// **Authentication & Authorization (Optional)**
builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  // Redirect to login if not authenticated
        options.LogoutPath = "/Auth/Logout"; // Redirect to logout
        options.AccessDeniedPath = "/Home/AccessDenied"; // Page for unauthorized access
    });

// **CORS Configuration (Optional)**
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow any origin
              .AllowAnyHeader()  // Allow any header
              .AllowAnyMethod(); // Allow any method (GET, POST, etc.)
    });
});

// **Optional: Add Logging** (for development and production)
builder.Services.AddLogging();

var app = builder.Build();

// Middleware mặc định
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Developer-specific error page
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Global error handling for production
    app.UseHsts();  // Security feature that forces HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable CORS globally (if needed)
app.UseCors("AllowAllOrigins");

app.UseRouting();

// **Authentication & Authorization Middleware**
app.UseSession();   // Session middleware
app.UseAuthentication(); // Authentication middleware (for Identity or custom auth)
app.UseAuthorization();  // Authorization middleware (if needed for roles/permissions)

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
