using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppServices.Middleware;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── HTTP Clients (Inter-service) ──────────────────────────────────────────
builder.Services.AddHttpClient("AuthService", client =>
{
    var url = builder.Configuration["Services:Auth"] ?? "http://localhost:5001/";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("X-Internal-Secret", "QuantityMeasurement_InternalSecret_2026!");
});

builder.Services.AddHttpClient("HistoryService", client =>
{
    var url = builder.Configuration["Services:History"] ?? "http://localhost:5003/";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("X-Internal-Secret", "QuantityMeasurement_InternalSecret_2026!");
});

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
                Message = "You must be logged in.",
                Details = context.AuthenticateFailure?.Message 
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

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>(); // Track requests across services

app.MapOpenApi();

// Modern documentation UI
app.UseSwagger();
app.UseSwaggerUI(); 

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
