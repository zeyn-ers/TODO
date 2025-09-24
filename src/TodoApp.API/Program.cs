// Program.cs
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application;
using TodoApp.Application.Mapping;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers + FluentValidation + Anti-forgery
builder.Services.AddControllers();
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Anti-forgery tokens for CSRF protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

// Validatorları tara
builder.Services.AddValidatorsFromAssemblyContaining<TodoApp.Application.Validators.CreateTodoDtoValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App & Infra
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// CORS (frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", p => p
        .WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

//
// 🔧 DB şemasını otomatik güncelle (veriyi SİLMEZ) + log
//
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
    var cs = db.Database.GetDbConnection().ConnectionString;
    app.Logger.LogInformation("✅ Connected DB: {ConnectionString}", cs);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// ⚙️ Güvenlik + içerik türü + cache header düzeltmeleri
app.Use(async (ctx, next) =>
{
    // Devam etsin, sonra header’ları set edelim
    await next();

    var h = ctx.Response.Headers;

    // --- Security headers ---
    h.TryAdd("X-Content-Type-Options", "nosniff");                 // Issues: x-content-type-options
    h.TryAdd("Referrer-Policy", "no-referrer");
    h.TryAdd("X-Frame-Options", "DENY");                           // Not: CSP ile güçlendirilir
    h.TryAdd("Content-Security-Policy", "frame-ancestors 'none'"); // iframe engeli

    // --- JSON charset fix (CORB/charset uyarıları için) ---
    if (!string.IsNullOrEmpty(ctx.Response.ContentType) &&
        ctx.Response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) &&
        !ctx.Response.ContentType.Contains("charset", StringComparison.OrdinalIgnoreCase))
    {
        ctx.Response.ContentType = "application/json; charset=utf-8";
    }

    // --- Pragma/Expires kaldır, Cache-Control tercih et ---
    h.Remove("Pragma");
    h.Remove("Expires");

    // API cevapları için güvenli cache politikası
    if (ctx.Request.Path.StartsWithSegments("/api"))
    {
        h["Cache-Control"] = "no-cache, no-store, must-revalidate";
    }
});

// (Statik dosya servis ediyorsan) Cache-Control’u modernleştir
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var h = ctx.Context.Response.Headers;
        // Build çıktıları için uzun cache (gerekirse düşür)
        h["Cache-Control"] = "public, max-age=604800, immutable";
        h.Remove("Pragma");
        h.Remove("Expires");
    }
});

app.UseCors("AllowFrontend");
app.UseAuthorization();

// Anti-forgery token endpoint
app.MapGet("/api/antiforgery/token", (IAntiforgery forgery, HttpContext context) =>
{
    var tokens = forgery.GetAndStoreTokens(context);
    return Results.Ok(new { token = tokens.RequestToken });
});

app.MapControllers();

app.Run();
