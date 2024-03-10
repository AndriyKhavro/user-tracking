namespace PixelService.Services;

public interface ITrackingService
{
    Task Track(HttpContext context);
}