using Microsoft.Extensions.Options;
using PixelService.Services;
using PixelService.Settings;
using StackExchange.Redis;

namespace PixelService;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            var redisConnectionString = new ConfigurationOptions
            {
                EndPoints = { redisSettings.Endpoint }
            }.ToString();

            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddTransient<IStorageService, RedisStorageService>();
        services.AddTransient<IPixelImageProvider, PixelImageProvider>();
        services.AddTransient<ITrackingService, TrackingService>();
        services.AddTransient<ITimeService, TimeService>();
    }
}