using System.Text.Json;
using Microsoft.Extensions.Options;
using PixelService.Models;
using PixelService.Settings;
using StackExchange.Redis;

namespace PixelService.Services;

public class RedisStorageService : IStorageService
{
    private readonly IDatabase _database;
    private readonly string _queueName;

    public RedisStorageService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisSettings> options)
    {
        _database = connectionMultiplexer.GetDatabase();
        _queueName = options.Value.QueueName;
    }

    public async Task Save(TrackingInfo trackingInfo)
    {
        var message = JsonSerializer.Serialize(trackingInfo);
        await _database.ListLeftPushAsync(_queueName, message);
    }
}