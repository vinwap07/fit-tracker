using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Infrastructure.FileService;

public static class FileStorageExtensions
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    { 
        var section = configuration.GetSection(MinioOptions.SectionName);
        services.Configure<MinioOptions>(section);

        services.AddScoped<IMinioClient>(sp =>
        {
            var options = section.Get<MinioOptions>();
            if (options == null)
            {
                throw new Exception("Minio options are missing.");
            }

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(false)
                .Build();
        });
        
        services.AddScoped<IFileStorageService, MinioStorageService>();
        return services;
    }
}