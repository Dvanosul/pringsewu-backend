using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.API.Middlewares;
using Sindika.AspNet.app015.Configuration;
using Sindika.AspNet.app015.Extensions;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.app015.Application.Services;
using Sindika.AspNet.Authentication.Extensions;
using Sindika.AspNet.Connection.Postgresql;
using Sindika.AspNet.Connection.Rabbitmq;
using Sindika.AspNet.Connection.Redis;
using Sindika.AspNet.Enrichment;
using Sindika.AspNet.Enrichment.Configuration;
using Sindika.AspNet.Enrichment.Enrichment.Middleware;
using Sindika.AspNet.Storage.Extensions;
using Sindika.AspNet.Storage.Interfaces;
using Sindika.AspNet.Validation;
using Sindika.AspNet.Validation.ActionFilter;
using Sindika.AspNet.Validation.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sindika.AspNet.Validation.Middleware;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Authentication.Services;
using Sindika.AspNet.Authentication.Middleware;
using Microsoft.AspNetCore.StaticFiles;
using Sindika.AspNet.SecretManager.Providers.Vault;
using Sindika.AspNet.Storage.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
LoggingConfiguration.ConfigureSerilog(builder);

// Configure Mapster
MapsterConfig.Configure();

// Load Configuration
var Configuration = builder.Configuration;
Configuration.AddEnvironmentVariables();

// Add Services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});

// Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.Audience = builder.Configuration["Authentication:Audience"];
    o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"] ?? "";
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
    };
});

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorsPolicy();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddAuthorization();
builder.Services.AddStorages(Configuration);

// Add External Connections
builder.Services.AddRedis(Configuration);
builder.Services.AddPostgresql(Configuration);
builder.Services.AddRabbitMQ(Configuration);
builder.Services.AddDbAuthorization();
builder.Services.AddSSOAuthentications(Configuration);

// Configure Enrichment
builder.Services.Configure<EnrichmentSettings>(Configuration.GetSection("EnrichmentSettings"));
builder.Services.AddEnrichment();

// Configure Validation
builder.Services.Configure<ValidationSettings>(Configuration.GetSection("ValidationSettings"));
builder.Services.AddValidation();

// Configure Model State
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Configure Database Context
builder.Services.AddDbContextFactory<Context>((serviceProvider, options) =>
{
    var connectionService = serviceProvider.GetRequiredService<PostgresqlConnectionManager>();
    var connectionString = connectionService.GetConnectionString();
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddTransient<IHostedService, AppSeederService>();
builder.Services.AddScoped<IContentTypeProvider, FileExtensionContentTypeProvider>();
builder.Services.Configure<VaultSettings>(Configuration.GetSection("VaultSettings"));

var app = builder.Build();

app.UseCors();

// Add Middleware
app.UseMiddleware<EnabledBufferingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<TransactionOptionMiddleware>();
app.UseMiddleware<InputValidationMiddleware>();
app.UseMiddleware<EnrichmentMiddleware>();

// Fetch secrets
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    await serviceProvider.GetRequiredService<PostgresqlConnectionManager>().FetchSecret();
    await serviceProvider.GetRequiredService<RedisConnectionManager>().FetchSecret();
    await serviceProvider.GetRequiredService<ISSOService>().FetchSecret();
    await serviceProvider.GetRequiredService<StorageSecretService>().FetchSecret();
}

// Health Checks
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    await serviceProvider.GetRequiredService<RedisHealthChecker>().CheckRedisHealthAsync();
    await serviceProvider.GetRequiredService<PostgresqlHealthChecker>().CheckPostgresqlHealthAsync();
    await serviceProvider.GetRequiredService<IStorageService>().TestConnectionAsync();
    await serviceProvider.GetRequiredService<IJwtService>().CheckHealthAsync();
    await serviceProvider.GetRequiredService<ISSOService>().CheckHealthAsync();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// HTTP Settings
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseHttpsRedirection();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers and Enable CORS
app.MapControllers();

// Application Lifetime Logger
var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Urls;

    if (addresses != null)
    {
        foreach (var address in addresses)
        {
            appLogger.LogInformation("Application is ready and listening for requests on {Address}", address);
        }
    }
    else
    {
        appLogger.LogInformation("Application is ready but no specific addresses could be retrieved.");
    }
});

// Run the Application
app.Run();
