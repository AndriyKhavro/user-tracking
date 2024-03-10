namespace StorageService.Services;

public class MessageProcessor : IMessageProcessor
{
    private readonly IMessageQueueClient _messageQueueClient;
    private readonly IMessageWriter _writer;
    private readonly IMessageSerializer _serializer;

    public MessageProcessor(IMessageQueueClient messageQueueClient, IMessageWriter writer, IMessageSerializer serializer)
    {
        _messageQueueClient = messageQueueClient;
        _writer = writer;
        _serializer = serializer;
    }

    public async Task ProcessOneMessageOrSleep(int millisecondsDelay)
    {
        var trackingInfo = await _messageQueueClient.ReadFromQueue();
        if (trackingInfo is null)
        {
            await Task.Delay(millisecondsDelay);
        }
        else
        {
            var message = _serializer.Serialize(trackingInfo);
            await _writer.WriteMessage(message);
        }

    }
}