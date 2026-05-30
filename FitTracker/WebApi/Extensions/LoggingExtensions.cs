using Serilog;

namespace Presentation.Extensions;

public static class LoggingExtensions
{
    public static void AddLogging(this ConfigureHostBuilder host, IConfiguration configuration)
    {
        var section = configuration.GetSection(LoggingOptions.SectionName);
        var options = section.Get<LoggingOptions>();
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(options.FileName, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        host.UseSerilog();
    }
}