using StorageService.Models;

namespace StorageService.Services;

internal class MessageSerializer : IMessageSerializer
{
    public string Serialize(TrackingInfo trackingInfo)
    {
        var dateTimeString = trackingInfo.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
        var referrer = GetValueOrNull(trackingInfo.Referer);
        var userAgent = GetValueOrNull(trackingInfo.UserAgent);
        var message = $"{dateTimeString}|{referrer}|{userAgent}|{trackingInfo.IpAddress}";
        return message;
    }
    private static string GetValueOrNull(string value) => string.IsNullOrEmpty(value) ? "null" : value;
}