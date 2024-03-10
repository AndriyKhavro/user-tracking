using Microsoft.AspNetCore.Mvc;
using PixelService;
using PixelService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/track", async (HttpContext context, [FromServices]ITrackingService trackingService, [FromServices]IPixelImageProvider pixelImageProvider) =>
{
    await trackingService.Track(context);

    context.Response.ContentType = "image/gif";
    await context.Response.Body.WriteAsync(pixelImageProvider.GetPixelImage());
});

app.Run();