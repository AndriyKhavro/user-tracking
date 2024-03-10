using StorageService.Models;

namespace StorageService.Services;

public interface IMessageQueueClient
{
    Task<TrackingInfo?> ReadFromQueue();
}