namespace Defra.PTS.Web.QRCoder.Services.Interfaces;

public interface IQRCodeService
{
    Task<byte[]> GetQRCode(string text, int pixelsPerModule = 10);

    Task<string> GetQRCodeAsBase64String(string text, int pixelsPerModule = 10);

    Task<string> GetQRCodeAsImageUrl(string text, int pixelsPerModule = 10);
}
