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
using Kukirmash.API.Extensions;
using Microsoft.AspNetCore.CookiePolicy;

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
    services.AddDbContext<KukirmashDbContext>( options => 
    { 
        options.UseNpgsql(config.GetConnectionString(nameof(KukirmashDbContext))); 
    });

    services.AddCors(options => {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
    });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddApiAuthentication(config);

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IServerRepository, ServerRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IServerService, ServerService>();
    services.AddScoped<IJwtProvider, JwtProvider>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();

    var app = builder.Build();

    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCookiePolicy( new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });

    app.UseAuthentication();
    app.UseAuthorization();



    app.AddMappedEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, $"Unexpected error: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}
