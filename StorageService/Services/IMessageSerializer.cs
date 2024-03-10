using StorageService.Models;

namespace StorageService.Services;

public interface IMessageSerializer
{
    string Serialize(TrackingInfo trackingInfo);
}