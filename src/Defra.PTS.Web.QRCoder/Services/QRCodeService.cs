using Defra.PTS.Web.QRCoder.Services.Interfaces;
using QRCoder;
using static QRCoder.QRCodeGenerator;

namespace Defra.PTS.Web.QRCoder.Services;

public class QRCodeService : IQRCodeService
{
    private readonly ECCLevel errorCorrectionLevel;
    public QRCodeService()
    {
        // High - Error correction level to be set to High (30%)
        errorCorrectionLevel = ECCLevel.H;
    }

    public async Task<byte[]> GetQRCode(string text, int pixelsPerModule = 4)
    {

        using var generator = new QRCodeGenerator();
        using var codeData = generator.CreateQrCode(text, errorCorrectionLevel);
        using var code = new PngByteQRCode(codeData);

        var byteArray = code.GetGraphic(pixelsPerModule);

        return await Task.FromResult(byteArray);
    }

    public async Task<string> GetQRCodeAsBase64String(string text, int pixelsPerModule = 4)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var byteArray = await GetQRCode(text, pixelsPerModule);
        return Convert.ToBase64String(byteArray);
    }

    public async Task<string> GetQRCodeAsImageUrl(string text, int pixelsPerModule = 4)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var base64String = await GetQRCodeAsBase64String(text, pixelsPerModule);
        return $"data:image/png;base64,{base64String}";
    }
}
