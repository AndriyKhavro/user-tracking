namespace PixelService.Settings;

public class RedisSettings
{
    public string Endpoint { get; init; } = null!;
    public string QueueName { get; init; } = null!;
}