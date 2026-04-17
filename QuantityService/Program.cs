using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.Services;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppServices.Services;
using QuantityService.Middleware;
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

// ── Services ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
builder.Services.AddScoped<IQuantityWebService, QuantityWebServiceImpl>();
builder.Services.AddHttpContextAccessor();

// HttpClient to call HistoryService (interservice communication)
builder.Services.AddHttpClient("HistoryService", client =>
{
    var url = builder.Configuration["Services:History"] ?? "https://qma-history-service.onrender.com/";
    
    // Ensure URL is clean for production
    if (!url.StartsWith("http")) url = "https://" + url;
    if (!url.EndsWith("/")) url += "/";

    client.BaseAddress = new Uri(url);
    Console.WriteLine($"[QuantityService Config] History Client initialized with BaseAddress: {url}");
});

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