using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Persistence.Repositories;
using Kukirmash.Application.Services;
using Kukirmash.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Kukirmash.Infrastructure;
using Kukirmash.API.Endpoints;
using Kukirmash.Infrastructure.JWT;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

try
{
    Log.Information("Starting kukirmash web application");

    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    var services = builder.Services;

    services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
    services.AddDbContext<KukirmashDbContext>(
        options => { options.UseNpgsql(config.GetConnectionString(nameof(KukirmashDbContext))); });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IJwtProvider, JwtProvider>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapUserEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Unexpected error");
}
finally
{
    Log.CloseAndFlush();
}
