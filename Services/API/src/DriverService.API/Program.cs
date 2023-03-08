using DriverService.API.BackgroundServices;
using DriverService.API.Configuration;
using DriverService.API.Filters;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Configuration.AddLoggerConfig();

builder.Services.AddDependencies();
builder.Services.AddApplicationSettings(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<DefaultExceptionFilterAttribute>();
    options.Filters.Add<ValidateModelFilterAttribute>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddHostedService<TracingRouteConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSwaggerConfig();

app.UseRouting();

app.UseEndpoints(options =>
{
    options.MapControllers();
});

app.Run();
