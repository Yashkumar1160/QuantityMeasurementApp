using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ── Service URLs ───────────────────────────────────────────────────────────
// In Docker, these will be set via Environment Variables:
var authServiceUrl     = builder.Configuration["Services:Auth"]     ?? "https://qma-auth-service-p1hw.onrender.com/";
var quantityServiceUrl = builder.Configuration["Services:Quantity"] ?? "https://qma-quantity-service.onrender.com/";
var historyServiceUrl  = builder.Configuration["Services:History"]  ?? "https://qma-history-service.onrender.com/";
var adminServiceUrl    = builder.Configuration["Services:Admin"]    ?? "https://qma-admin-service.onrender.com/";

// ── HTTP clients ───────────────────────────────────────────────────────────
// Using IHttpClientFactory is the proper way to handle outbound HTTP calls.
builder.Services.AddHttpClient("auth-service",     c => { c.BaseAddress = new Uri(authServiceUrl);     Console.WriteLine($"[Gateway] Auth Client -> {authServiceUrl}"); });
builder.Services.AddHttpClient("quantity-service", c => { c.BaseAddress = new Uri(quantityServiceUrl); Console.WriteLine($"[Gateway] Quantity Client -> {quantityServiceUrl}"); });
builder.Services.AddHttpClient("history-service",  c => { c.BaseAddress = new Uri(historyServiceUrl);  Console.WriteLine($"[Gateway] History Client -> {historyServiceUrl}"); });
builder.Services.AddHttpClient("admin-service",    c => { c.BaseAddress = new Uri(adminServiceUrl);    Console.WriteLine($"[Gateway] Admin Client -> {adminServiceUrl}"); });

// ── OpenAPI & Swagger ─────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ── Swagger Documentation UI ────────────────────────────────────────────────
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Local Gateway API (Shows the raw Gateway routes)
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "System Overview (Gateway)");

    // Aggregated Microservice APIs
    options.SwaggerEndpoint("/swagger-docs/auth",     "Auth Service API");
    options.SwaggerEndpoint("/swagger-docs/quantity", "Quantity Service API");
    options.SwaggerEndpoint("/swagger-docs/history",  "History Service API");
    options.SwaggerEndpoint("/swagger-docs/admin",    "Admin Service API");

    options.DocumentTitle = "QMA - Unified Microservices API";
});

app.UseCors("AllowAngular");

// ═══════════════════════════════════════════════════════════════════════════
// HELPER: The "Relay" logic that forwards requests to downstream services.
// ═══════════════════════════════════════════════════════════════════════════
static async Task ForwardAsync(HttpContext ctx, IHttpClientFactory factory, string clientName, string path)
{
    var client = factory.CreateClient(clientName);
    var method = new HttpMethod(ctx.Request.Method);
    
    // Create the message to send to the downstream service
    var request = new HttpRequestMessage(method, path + ctx.Request.QueryString);

    Console.WriteLine($"[Gateway] Forwarding {ctx.Request.Method} {ctx.Request.Path} to {client.BaseAddress}{path}");

    // Forward the Authorization header (JWT) if it exists
    if (ctx.Request.Headers.TryGetValue("Authorization", out var auth))
    {
        request.Headers.TryAddWithoutValidation("Authorization", auth.ToString());
    }

    // Forward the Correlation ID (for tracing)
    if (ctx.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
    {
        request.Headers.TryAddWithoutValidation("X-Correlation-ID", correlationId.ToString());
    }

    // Forward the request body (POST/PUT data) by buffering it first.
    // This avoids stream deadlocks in custom Gateway implementations.
    if (ctx.Request.ContentLength > 0 || ctx.Request.Headers.ContainsKey("Transfer-Encoding"))
    {
        var buffer = new MemoryStream();
        await ctx.Request.Body.CopyToAsync(buffer);
        request.Content = new ByteArrayContent(buffer.ToArray());
        
        if (ctx.Request.ContentType != null)
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(ctx.Request.ContentType);
    }

    try
    {
        var response = await client.SendAsync(request);
        ctx.Response.StatusCode = (int)response.StatusCode;
        
        // List of hop-by-hop headers that should NOT be forwarded back to the client
        var restrictedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Transfer-Encoding", "Connection", "Keep-Alive", "Upgrade", "Proxy-Authenticate", 
            "Proxy-Authorization", "TE", "Trailer"
        };

        // Copy response headers (skipping restricted ones)
        foreach (var header in response.Headers)
        {
            if (!restrictedHeaders.Contains(header.Key))
                ctx.Response.Headers[header.Key] = header.Value.ToArray();
        }

        // Also copy Content headers (like Content-Type)
        foreach (var header in response.Content.Headers)
        {
            if (!restrictedHeaders.Contains(header.Key))
                ctx.Response.Headers[header.Key] = header.Value.ToArray();
        }

        await response.Content.CopyToAsync(ctx.Response.Body);
    }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = 502; // Bad Gateway
        await ctx.Response.WriteAsJsonAsync(new { error = "Gateway Error", message = ex.Message });
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// ROUTES: Mapping incoming urls to downstream services
// ═══════════════════════════════════════════════════════════════════════════

// Auth Service Routes
app.MapGroup("/api/v1/auth").Map("{*any}", (HttpContext ctx, IHttpClientFactory f) => 
    ForwardAsync(ctx, f, "auth-service", ctx.Request.Path)).WithTags("Auth");

// Quantity Service Routes
app.MapGroup("/api/v1/quantities").Map("{*any}", (HttpContext ctx, IHttpClientFactory f) => 
    ForwardAsync(ctx, f, "quantity-service", ctx.Request.Path)).WithTags("Quantity");

// History Service Routes
app.MapGroup("/api/v1/history").Map("{*any}", (HttpContext ctx, IHttpClientFactory f) => 
    ForwardAsync(ctx, f, "history-service", ctx.Request.Path)).WithTags("History");

// Admin Service Routes
app.MapGroup("/api/v1/admin").Map("{*any}", (HttpContext ctx, IHttpClientFactory f) => 
    ForwardAsync(ctx, f, "admin-service", ctx.Request.Path)).WithTags("Admin");

// ── Swagger Proxy Routes (Relays documentation from internal services) ──────
app.MapGet("/swagger-docs/auth",     (HttpContext ctx, IHttpClientFactory f) => ForwardAsync(ctx, f, "auth-service",     "/swagger/v1/swagger.json"));
app.MapGet("/swagger-docs/quantity", (HttpContext ctx, IHttpClientFactory f) => ForwardAsync(ctx, f, "quantity-service", "/swagger/v1/swagger.json"));
app.MapGet("/swagger-docs/history",  (HttpContext ctx, IHttpClientFactory f) => ForwardAsync(ctx, f, "history-service",  "/swagger/v1/swagger.json"));
app.MapGet("/swagger-docs/admin",    (HttpContext ctx, IHttpClientFactory f) => ForwardAsync(ctx, f, "admin-service",    "/swagger/v1/swagger.json"));

// ── Health Check ───────────────────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok(new { status = "Gateway Healthy", time = DateTime.UtcNow }));

app.Run();