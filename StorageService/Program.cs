using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelService;
using StorageService.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.ConfigureServices(configuration);

var sp = services.BuildServiceProvider();

var messageProcessor = sp.GetRequiredService<IMessageProcessor>();

while (true)
{
    await messageProcessor.ProcessOneMessageOrSleep(100);
}

