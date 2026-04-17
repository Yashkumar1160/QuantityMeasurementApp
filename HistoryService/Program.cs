using System.Text;
using HistoryService.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using System.IdentityModel.Tokens.Jwt;
using QuantityMeasurementAppServices.Middleware;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Database ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<HistoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
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
    // This creates the database and tables if they don't exist
    db.Database.EnsureCreated();
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