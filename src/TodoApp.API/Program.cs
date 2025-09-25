using System.Text.Json.Serialization;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using TodoApp.Infrastructure;
using TodoApp.Application;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("App", "TodoApp")
    .WriteTo.Console()
    .WriteTo.File("logs/todoapp-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// ===== Services =====
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper (Application -> Mappings/MappingProfile.cs)
builder.Services.AddAutoMapper(typeof(TodoApp.Application.Mappings.MappingProfile).Assembly);

// Controllers + JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .ConfigureApiBehaviorOptions(o =>
    {
        o.InvalidModelStateResponseFactory = ctx =>
            new BadRequestObjectResult(new ValidationProblemDetails(ctx.ModelState));
    });

// API versioning + explorer
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(2, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (appsettings: Cors:Allowed -> string[])
builder.Services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("Cors:Allowed").Get<string[]>() ?? Array.Empty<string>();
    if (origins.Length == 0)
        options.AddPolicy("Default", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    else
        options.AddPolicy("Default", p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// ===== Pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "TodoApp API v2");
        c.RoutePrefix = string.Empty;
    });
}

// Tek tip global hata yanıtı
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async ctx =>
    {
        var feature = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var problem = new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = app.Environment.IsDevelopment() ? feature?.Error.Message : "Unexpected error",
            Instance = ctx.Request.Path
        };
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = problem.Status!.Value;
        await ctx.Response.WriteAsJsonAsync(problem);
    });
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Default");
app.MapControllers();

app.Run();
