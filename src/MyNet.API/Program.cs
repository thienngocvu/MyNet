using MyNet.API.Extensions;
using MyNet.API.Middlewares;
using MyNet.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container
builder.Services
    .AddCustomControllerExt()
    .AddAuthenticationExt(configuration)
    .AddRedisCacheExt(configuration)
    .AddHealthChecksExt(configuration)
    .AddFluentValidationExt()
    .AddAutoMapperExt()
    .AddServiceExt(configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerExt();

builder.Host.AddSerilogExt(configuration);

var app = builder.Build();

// Apply migrations and seed database
await app.Services.SeedDatabaseAsync();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExt();
}

app.UseHttpsRedirection();

app.UseRouting()
    .UseAuthenticationExt()
    .UseAllowedHostExt();

app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/api/auth")
        && !context.Request.Path.StartsWithSegments("/swagger")
        && !context.Request.Path.StartsWithSegments("/health"),
    appBuilder =>
    {
        appBuilder.UseMiddleware<UserContextMiddleware>();
    });

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.UseHealthChecksExt();
});

app.Run();
