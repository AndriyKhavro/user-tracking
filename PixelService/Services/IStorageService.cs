using PixelService.Models;

namespace PixelService.Services;

public interface IStorageService
{
    Task Save(TrackingInfo trackingInfo);
}