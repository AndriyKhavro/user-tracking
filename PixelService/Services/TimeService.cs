namespace PixelService.Services;

public class TimeService : ITimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}