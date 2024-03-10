namespace StorageService.Services;

public interface IMessageProcessor
{
    Task ProcessOneMessageOrSleep(int millisecondsDelay);
}