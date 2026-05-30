using Application.Abstractions;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture.Jobs.Hangfire;

public static class BackgroundJobsExtensions
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(HangfireOptions.SectionName).Get<HangfireOptions>();

        if (options is null)
        {
            throw new Exception("Hangfire settings are not configured.");
        }

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(postgreOptions =>
            {
                postgreOptions.UseNpgsqlConnection(options.ConnectionString);
            }, new PostgreSqlStorageOptions()
            {
                SchemaName = options.SchemaName,
                PrepareSchemaIfNecessary = true,
            }));
        
        services.AddHangfireServer();

        services.AddScoped<IJobService, HangfireJobService>();
        
        return services;
    }
}