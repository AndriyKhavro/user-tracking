namespace StorageService.Services;

public interface IMessageWriter
{
    Task WriteMessage(string message);
}