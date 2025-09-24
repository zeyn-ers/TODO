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

// Validatorları tara (uygulamadaki bir validator tipinden keşfeder)
builder.Services.AddValidatorsFromAssemblyContaining<TodoApp.Application.Validators.CreateTodoDtoValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App & Infra
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// CORS - More secure configuration
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
// 🔧 DB şemasını otomatik güncelle (veriyi SİLMEZ)
//    ve hangi DB'ye bağlı olduğunu logla
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
app.UseCors("AllowFrontend");
app.UseAuthorization();

// Add anti-forgery token endpoint
app.MapGet("/api/antiforgery/token", (IAntiforgery forgery, HttpContext context) =>
{
    var tokens = forgery.GetAndStoreTokens(context);
    return Results.Ok(new { token = tokens.RequestToken });
});

app.MapControllers();

app.Run();
