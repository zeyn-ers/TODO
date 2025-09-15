using FluentValidation.AspNetCore;
using TodoApp.Application;
using TodoApp.Infrastructure;

// WebApplication Builder oluştur - .NET 6+ minimal hosting model
var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES CONFIGURATION =====
// DI Container'a servisleri ekle

// MVC Controllers ekle - FluentValidation ile birlikte
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<TodoApp.Application.Validators.CreateTodoDtoValidator>();
    });

// API Explorer ve Swagger/OpenAPI konfigürasyonu
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "TodoApp API", 
        Version = "v1",
        Description = "A simple Todo API built with N-Layer Architecture"
    });
    
    // XML yorumlarını Swagger'a dahil et (API dokümantasyonu için)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Custom servisleri ekle (N-Layer Architecture)
builder.Services.AddApplication(); // Application katmanı servisleri
builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure katmanı servisleri

// CORS (Cross-Origin Resource Sharing) konfigürasyonu
// Frontend uygulamalarının API'ye erişmesine izin verir
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Tüm origin'lere izin ver
              .AllowAnyMethod()    // Tüm HTTP metodlarına izin ver
              .AllowAnyHeader();   // Tüm header'lara izin ver
    });
});

// ===== APPLICATION BUILD =====
var app = builder.Build();

// ===== MIDDLEWARE PIPELINE =====
// HTTP request pipeline'ını konfigüre et

// Development ortamında Swagger UI'yi etkinleştir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API V1");
        c.RoutePrefix = string.Empty; // Swagger UI'yi root path'te göster
    });
}

// HTTPS yönlendirmesi (güvenlik için)
app.UseHttpsRedirection();

// CORS middleware'ini etkinleştir
app.UseCors("AllowAll");

// Authorization middleware (şu an kullanılmıyor ama gelecekte eklenebilir)
app.UseAuthorization();

// Controller endpoint'lerini map et
app.MapControllers();

// Uygulamayı başlat
app.Run();
