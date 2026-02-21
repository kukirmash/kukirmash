using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Application.Interfaces;

using Kukirmash.Persistence.Repositories;
using Kukirmash.Application.Services;
using Kukirmash.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Kukirmash.Infrastructure;
using FluentValidation;
using Kukirmash.Infrastructure.JWT;
using Kukirmash.API.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Kukirmash.API.Hubs;



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

try
{
    Log.Information("Starting kukirmash web application");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    var services = builder.Services;

    services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
    services.AddDbContext<KukirmashDbContext>(options =>
    {
        options.UseNpgsql(config.GetConnectionString(nameof(KukirmashDbContext)));
    });

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
    });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddApiAuthentication(config);
    // services.AddAntiforgery();

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IServerRepository, ServerRepository>();
    services.AddScoped<ITextChannelRepository, TextChannelRepository>();
    services.AddScoped<ITextMessageRepository, TextMessageRepository>();

    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IServerService, ServerService>();
    services.AddScoped<ITextChannelService, TextChannelService>();
    services.AddScoped<ITextMessageService, TextMessageService>();

    services.AddScoped<IJwtProvider, JwtProvider>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddScoped<IStaticFileService, StaticFileService>();

    services.AddSignalR();
    services.AddValidatorsFromAssemblyContaining<Program>();
    //services.AddFluentValidationRulesToSwagger();

    WebApplication app = builder.Build();

    app.UseStaticFiles();
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });

    app.UseAuthentication();
    app.UseAuthorization();

    // app.UseAntiforgery();

    app.AddMappedEndpoints();
    app.MapHub<TextChannelHub>("/text-channels");

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
