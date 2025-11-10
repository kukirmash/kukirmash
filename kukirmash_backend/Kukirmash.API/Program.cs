using Kukirmash.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

try
{
    Log.Information("Starting kukirmash web application");

    var builder = WebApplication.CreateBuilder(args);

    var config = builder.Configuration;

    builder.Services.AddDbContext<KukirmashDbContext>(
        options =>
        {
            options.UseNpgsql(config.GetConnectionString(nameof(KukirmashDbContext)));
        });

    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");

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
