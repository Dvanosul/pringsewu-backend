using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Middlewares;
using Sindika.AspNet.app015.Configuration;
using Sindika.AspNet.app015.Extensions;
using Sindika.AspNet.Connection.Redis;
using Sindika.AspNet.Midtrans.Extensions;
using Sindika.AspNet.Validation;
using Sindika.AspNet.Validation.ActionFilter;
using Sindika.AspNet.Validation.Configuration;
using Sindika.AspNet.Validation.Middleware;

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
}).AddMidtrans();

builder.Services.AddMidtrans(options =>
{
    builder.Configuration.GetSection("Midtrans").Bind(options);
});


builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorsPolicy();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddServices();

// Add External Connections
builder.Services.AddRedis(Configuration);

// Configure Validation
builder.Services.Configure<ValidationSettings>(Configuration.GetSection("ValidationSettings"));
builder.Services.AddValidation();

// Configure Model State
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseCors();

// Add Middleware
app.UseMiddleware<EnabledBufferingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
// Skip authentication/authorization middleware for now
app.UseMiddleware<InputValidationMiddleware>();

// Fetch secrets & Health Checks
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    await serviceProvider.GetRequiredService<RedisConnectionManager>().FetchSecret();
    await serviceProvider.GetRequiredService<RedisHealthChecker>().CheckRedisHealthAsync();
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


// Midtrans notifications middleware (webhook handling)
app.UseMidtransNotifications();

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
