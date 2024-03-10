using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StorageService.Services;
using StorageService.Settings;

namespace PixelService;

public static class DependencyInjection
{
    private const string DefaultFilePath = "/tmp/visits.log";

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.Configure<FileSystemSettings>(settings => settings.PathToFile = configuration["FilePath"] ?? DefaultFilePath);

        services.AddSingleton(configuration);
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            var redisConnectionString = new ConfigurationOptions
            {
                EndPoints = { redisSettings.Endpoint }
            }.ToString();

            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddTransient<IMessageQueueClient, RedisMessageQueueClient>();
        services.AddTransient<IMessageWriter, MessageWriter>();
        services.AddTransient<IMessageProcessor, MessageProcessor>();
        services.AddTransient<IMessageSerializer, MessageSerializer>();
    }
}