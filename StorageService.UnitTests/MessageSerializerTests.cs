using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelService;
using StorageService.Models;
using StorageService.Services;

namespace StorageService.UnitTests;

public class MessageSerializerTests
{
    private readonly IMessageSerializer _sut;

    public MessageSerializerTests()
    {
        var configuration = new ConfigurationBuilder()
            .Build();

        var services = new ServiceCollection();

        services.ConfigureServices(configuration);
        var sp = services.BuildServiceProvider();
        _sut = sp.GetRequiredService<IMessageSerializer>();
    }

    [Fact]
    public void Serialize_ReturnsMessageInCorrectFormat()
    {
        var trackingInfo = new TrackingInfo(
            new DateTime(2022, 12, 19, 14, 17, 49, 960, DateTimeKind.Utc), 
            "10.0.0.1", 
            "google.com",
            "AnotherUserAgent");

        var message = _sut.Serialize(trackingInfo);

        message.Should().Be("2022-12-19T14:17:49.9600000Z|google.com|AnotherUserAgent|10.0.0.1");
    }

    [Fact]
    public void Serialize_ReplacesRefererWithNull_IfEmpty()
    {
        var trackingInfo = new TrackingInfo(
            new DateTime(2022, 12, 19, 14, 17, 49, 960, DateTimeKind.Utc),
            "10.0.0.1",
            string.Empty,
            "AnotherUserAgent");

        var message = _sut.Serialize(trackingInfo);

        message.Should().Be("2022-12-19T14:17:49.9600000Z|null|AnotherUserAgent|10.0.0.1");
    }

    [Fact]
    public void Serialize_ReplacesUserAgentWithNull_IfEmpty()
    {
        var trackingInfo = new TrackingInfo(
            new DateTime(2022, 12, 19, 14, 17, 49, 960, DateTimeKind.Utc),
            "10.0.0.1",
            "bing.com",
            string.Empty);

        var message = _sut.Serialize(trackingInfo);

        message.Should().Be("2022-12-19T14:17:49.9600000Z|bing.com|null|10.0.0.1");
    }
}