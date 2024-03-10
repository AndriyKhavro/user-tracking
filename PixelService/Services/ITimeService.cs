namespace PixelService.Services;

public interface ITimeService
{
    DateTime UtcNow { get; }
}