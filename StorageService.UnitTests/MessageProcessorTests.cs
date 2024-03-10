using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PixelService;
using StorageService.Models;
using StorageService.Services;

namespace StorageService.UnitTests;

public class MessageProcessorTests
{
    private readonly IMessageQueueClient _messageQueueClient = Substitute.For<IMessageQueueClient>();
    private readonly IMessageWriter _writer = Substitute.For<IMessageWriter>();
    private readonly IMessageProcessor _sut;

    public MessageProcessorTests()
    {
        var configuration = new ConfigurationBuilder()
            .Build();

        var services = new ServiceCollection();

        services.ConfigureServices(configuration);
        services.AddSingleton(_ => _messageQueueClient);
        services.AddSingleton(_ => _writer);
        var sp = services.BuildServiceProvider();
        _sut = sp.GetRequiredService<IMessageProcessor>();
    }

    [Fact]
    public async Task ProcessOneMessageOrSleep_CallsMessageWriter_IfMessageIsInQueue()
    {
        var trackingInfo = new TrackingInfo(
            new DateTime(2022, 12, 19, 14, 17, 49, 960, DateTimeKind.Utc),
            "10.0.0.1",
            "google.com",
            "AnotherUserAgent");

        _messageQueueClient.ReadFromQueue().Returns(trackingInfo);

        await _sut.ProcessOneMessageOrSleep(0);

        await _writer.Received().WriteMessage(Arg.Any<string>());
    }

    [Fact]
    public async Task ProcessOneMessageOrSleep_DoesNotCallMessageWriter_IfMessageIsNotInQueue()
    {
        _messageQueueClient.ReadFromQueue().Returns((TrackingInfo?)null);

        await _sut.ProcessOneMessageOrSleep(0);

        await _writer.DidNotReceive().WriteMessage(Arg.Any<string>());
    }
}