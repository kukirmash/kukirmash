using FluentValidation;
using Kukirmash.API.Extensions;
using Kukirmash.API.Hubs;
using Kukirmash.Application.Interfaces;
using Kukirmash.Application.Interfaces.Auth;
using Kukirmash.Application.Interfaces.Repositories;
using Kukirmash.Application.Interfaces.Services;
using Kukirmash.Application.Services;
using Kukirmash.Infrastructure;
using Kukirmash.Infrastructure.JWT;
using Kukirmash.Persistence;
using Kukirmash.Persistence.Repositories;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override( "Microsoft", LogEventLevel.Warning )
    .MinimumLevel.Override( "System", LogEventLevel.Warning )
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File("log.txt")
    .CreateLogger();

try
{
    Log.Information("Starting kukirmash web application");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

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

    using ( var scope = app.Services.CreateScope() )
    {
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var context = serviceProvider.GetRequiredService<KukirmashDbContext>();
            context.Database.Migrate();
        }
        catch ( Exception ex )
        {
            Log.Error( ex, "Ошибка при миграции базы данных." );
        }
    }

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
