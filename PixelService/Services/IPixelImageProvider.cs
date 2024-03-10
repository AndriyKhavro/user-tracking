namespace PixelService.Services;

public interface IPixelImageProvider
{
    ReadOnlyMemory<byte> GetPixelImage();
}