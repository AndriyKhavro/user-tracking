using System.Text;

namespace PixelService.Services;

public class PixelImageProvider : IPixelImageProvider
{
    private static readonly ReadOnlyMemory<byte> CachedPixelImage = GetTransparentPixelImage();

    public ReadOnlyMemory<byte> GetPixelImage() => CachedPixelImage;

    private static byte[] GetTransparentPixelImage()
    {
        const string gifHeader = "GIF89a\x01\x00\x01\x00\x80\x00\x00\xFF\xFF\xFF\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00,\x00\x00\x00\x00\x01\x00\x01\x00\x00\x02\x02D\x01\x00;";
        return Encoding.ASCII.GetBytes(gifHeader);
    }
}