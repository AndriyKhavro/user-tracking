using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PixelService.Models;
using PixelService.Services;

namespace PixelService.UnitTests;

public class TrackingServiceTests
{
    private readonly ITrackingService _sut;
    private readonly ITimeService _timeService = Substitute.For<ITimeService>();
    private readonly IStorageService _storageService = Substitute.For<IStorageService>();
    private readonly HttpContext _httpContent = Substitute.For<HttpContext>();

    public TrackingServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .Build();

        var services = new ServiceCollection();

        services.ConfigureServices(configuration);
        services.AddSingleton(_ => _storageService);
        services.AddSingleton(_ => _timeService);

        var serviceProvider = services.BuildServiceProvider();
        _sut = serviceProvider.GetRequiredService<ITrackingService>();
    }

    [Fact]
    public async Task Track_CallsStorageServiceWithCorrectObject_IfAllHeadersAreSet()
    {
        var date = new DateTime(2022, 12, 19, 14, 16, 49);
        var ipString = "192.168.1.1";
        var referer = "https://google.com";
        var userAgent = "SomeUserAgent";

        _timeService.UtcNow.Returns(date);

        _httpContent.Request.Headers.Returns(new HeaderDictionary
        {
            { "Referer", referer },
            { "User-Agent", userAgent }
        });

        _httpContent.Connection.RemoteIpAddress.Returns(IPAddress.Parse(ipString));

        await _sut.Track(_httpContent);

        await _storageService.Received().Save(new TrackingInfo(date, ipString, referer, userAgent));
    }

    [Fact]
    public async Task Track_CallsStorageServiceWithCorrectObject_IfHeadersAreMissing()
    {
        var date = new DateTime(2022, 12, 19, 14, 16, 49);
        var ipString = "10.0.0.1";

        _timeService.UtcNow.Returns(date);

        _httpContent.Request.Headers.Returns(new HeaderDictionary());

        _httpContent.Connection.RemoteIpAddress.Returns(IPAddress.Parse(ipString));

        await _sut.Track(_httpContent);

        await _storageService.Received().Save(new TrackingInfo(date, ipString, string.Empty, string.Empty));
    }

    [Fact]
    public async Task Track_DoesNotCallStorageService_IfIpAddressIsMissing()
    {
        var date = new DateTime(2022, 12, 19, 14, 16, 49);

        _timeService.UtcNow.Returns(date);

        _httpContent.Request.Headers.Returns(new HeaderDictionary());

        _httpContent.Connection.RemoteIpAddress.Returns((IPAddress?)null);

        await _sut.Track(_httpContent);

        await _storageService.DidNotReceive().Save(Arg.Any<TrackingInfo>());
    }
}