using Microsoft.Extensions.Options;
using StorageService.Settings;

namespace StorageService.Services;

internal class MessageWriter : IMessageWriter
{
    private readonly string _pathToFile;

    public MessageWriter(IOptions<FileSystemSettings> options)
    {
        _pathToFile = options.Value.PathToFile;
    }

    public async Task WriteMessage(string message)
    {
        await File.AppendAllTextAsync(_pathToFile, message + Environment.NewLine);
    }
}