using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using PropertyExperts.API.Logging;
using PropertyExperts.API.Middlewares;
using PropertyExperts.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Configure Serilog Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
SerilogConfig.ConfigureLogging();

//Add Services
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Register Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PropertyExpertsTask API", Version = "v1" });
});

//Register Dependencies
builder.Services.AddApplicationServices();

var app = builder.Build();

//Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

//Required for integration testing
public partial class Program { }
