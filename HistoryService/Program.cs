using System.Text;
using HistoryService.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using System.IdentityModel.Tokens.Jwt;
using QuantityMeasurementAppServices.Middleware;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Fix for PostgreSQL DateTime compatibility
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

builder.Services.AddDbContext<HistoryDbContext>(options =>
    options.UseNpgsql(connectionString));
    
builder.Services.AddScoped<IQuantityRecordRepository>(sp => new QuantityRecordRepository(sp.GetRequiredService<HistoryDbContext>()));
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
    options.UseSecurityTokenValidators = true;
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
builder.Services.AddControllers();

// ── OpenAPI & Swagger ─────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// ── Database Initialization (Auto-Create for Docker) ──────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HistoryDbContext>();
    // First, ensure the database itself exists
    db.Database.EnsureCreated();
    
    // SECOND: Explicitly force creation of even if db exists (EnsureCreated skips if DB exists)
    var databaseCreator = Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions.GetService<Microsoft.EntityFrameworkCore.Storage.IDatabaseCreator>(db.Database);
    if (databaseCreator is Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator relationalCreator)
    {
        try { relationalCreator.CreateTables(); } catch { /* Table already exists */ }
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