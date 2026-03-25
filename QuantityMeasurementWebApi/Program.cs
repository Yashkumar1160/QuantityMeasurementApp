using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Services;
using QuantityMeasurementAppRepositories.Context;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppServices.Services;
using QuantityMeasurementWebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ================== DATABASE ==================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================== DEPENDENCY INJECTION ==================
builder.Services.AddScoped<IQuantityRecordRepository, QuantityRecordRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
builder.Services.AddScoped<IQuantityWebService, QuantityWebServiceImpl>();

// ================== CONTROLLERS + GLOBAL ERROR HANDLING ==================
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

        var response = new
        {
            Timestamp = DateTime.UtcNow.ToString("o"),
            Status = 400,
            Error = "Validation Failed",
            Message = string.Join("; ", errors),
            Path = context.HttpContext.Request.Path.ToString()
        };

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});

// ================== SWAGGER ==================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "QuantityMeasurementAppWebAPI.xml");

    if (File.Exists(xmlFile))
        options.IncludeXmlComments(xmlFile);
});

// ================== HEALTH CHECK ==================
builder.Services.AddHealthChecks();

var app = builder.Build();

// ================== DATABASE MIGRATION ==================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ================== MIDDLEWARE PIPELINE ==================
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
