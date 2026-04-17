using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AuthService.Context;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppServices.Services;
using AuthService.Middleware;
using QuantityMeasurementAppServices.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Database ──────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = connectionString.Trim('"', ' ');
    if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var db = uri.LocalPath.TrimStart('/');
        var user = Uri.UnescapeDataString(userInfo[0]);
        var pass = Uri.UnescapeDataString(userInfo.Length > 1 ? userInfo[1] : "");

        connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass};Ssl Mode=Require;Trust Server Certificate=true;";
    }
}

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

// ── Services ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(sp.GetRequiredService<AuthDbContext>()));
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, QuantityMeasurementAppServices.Services.AuthService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<HashingService>();
builder.Services.AddHttpContextAccessor();

// ── Authentication ────────────────────────────────────────────────────────
string jwtSecretKey = builder.Configuration["Jwt:SecretKey"]!;
string jwtIssuer    = builder.Configuration["Jwt:Issuer"]!;
string jwtAudience  = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidIssuer              = jwtIssuer,
        ValidateAudience         = true,
        ValidAudience            = jwtAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.FromMinutes(5)
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode  = 401;
            context.Response.ContentType = "application/json";
            string body = System.Text.Json.JsonSerializer.Serialize(new
            {
                Status  = 401,
                Error   = "Unauthorized",
                Message = "You must be logged in."
            });
            await context.Response.WriteAsync(body);
        }
    };
});

builder.Services.AddAuthorization();

// ── Controllers & Exceptions ──────────────────────────────────────────────
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionHandler>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        var response = new { Status = 400, Error = "Validation Failed", Message = string.Join("; ", errors) };
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});

// ── OpenAPI & Swagger ─────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// ── Database Initialization (Auto-Create & Seed for Docker) ───────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    // This creates the database and tables if they don't exist
    db.Database.EnsureCreated();

    // Auto-seed a default Admin user if none exists
    if (!db.Users.Any(u => u.Role == "Admin"))
    {
        var admin = new QuantityMeasurementAppModels.Entities.UserEntity
        {
            Name = "System Admin",
            Email = "admin@quantities.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>(); // Track requests across services

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(); // Modern documentation UI

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();