using FluentValidation;
using FluentValidation.AspNetCore;
using LS.API.FluentValidations;
using LS.API.Middleware;
using LS.Application.DIConfiguration;
using LS.Application.DIConfiguration.Hosted;
using LS.Application.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog logging.
LoggingConfiguration.ConfigureSerilog(builder);

// Register application services (DI setup).
builder.Services.AddServices();
builder.Services.AddMediatr();

// Configure additional logging factories for use in services.
var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Configure database context with logger.
builder.Services.AddDbContext(builder.Configuration, loggerFactory.CreateLogger("DbContextConfiguration"));

// Register controllers and basic API features (Swagger, etc.).
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Configure caching services.
builder.Services.AddCache(builder.Configuration);

// Verify cache connection on startup.
builder.Services.AddHostedService<CacheConnectionValidationService>();

// Configure AutoMapper profiles and logging.
builder.Services.AddAutoMappers(loggerFactory.CreateLogger("AutoMappersRegistration"));

// Setup FluentValidation.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EarnPointsRequestValidator>();

// Setup health checks.
builder.Services.AddHealthChecks();

// Setup JWT-based authentication.
builder.Services.AddJWTAuthentication(loggerFactory.CreateLogger("JWTConfiguration"), builder.Configuration);

// Setup authorization policies.
builder.Services.AddAuthorization();

var app = builder.Build();

Console.WriteLine("Starting...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handling middleware.
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint.
app.MapHealthChecks("/health");

app.MapControllers();

// Start the application.
app.Run();
