using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StorageService.Models;
using StorageService.Settings;

namespace StorageService.Services;

internal class RedisMessageQueueClient : IMessageQueueClient
{
    private readonly IDatabase _database;
    private readonly string _queueName;

    public RedisMessageQueueClient(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisSettings> options)
    {
        _database = connectionMultiplexer.GetDatabase();
        _queueName = options.Value.QueueName;
    }

    public async Task<TrackingInfo?> ReadFromQueue()
    {
        var value = await _database.ListRightPopAsync(_queueName);
        if (value.IsNullOrEmpty)
        {
            return null;
        }

        var message = JsonSerializer.Deserialize<TrackingInfo>(value.ToString());
        return message;
    }
}