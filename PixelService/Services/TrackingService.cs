using PixelService.Models;

namespace PixelService.Services;

internal class TrackingService : ITrackingService
{
    private readonly IStorageService _storageService;
    private readonly ITimeService _timeService;

    public TrackingService(IStorageService storageService, ITimeService timeService)
    {
        _storageService = storageService;
        _timeService = timeService;
    }

    public async Task Track(HttpContext context)
    {
        var referer = context.Request.Headers.Referer.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var ipAddress = context.Connection.RemoteIpAddress;

        if (ipAddress is not null)
        {
            var trackingInfo = new TrackingInfo(_timeService.UtcNow, ipAddress.ToString(), referer, userAgent);
            await _storageService.Save(trackingInfo);
        }
    }
}