using Defra.PTS.Web.QRCoder.Services;
using Defra.PTS.Web.QRCoder.Services.Interfaces;
using FluentAssertions;

namespace Defra.PTS.Web.QRCoder.UnitTests.Services;

public class QRCodeServiceTests
{
    private readonly IQRCodeService qrCodeService;
    public QRCodeServiceTests()
    {
        qrCodeService = new QRCodeService();
    }

    [Fact]
    public async Task GetQRCode_ReturnsQRCodeAsBytesArray()
    {
        var text = "MockText";

        var result = await qrCodeService.GetQRCode(text, 10);

        result.Should().BeOfType<byte[]>();
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GetQRCodeAsBase64String_ReturnsQRCodeAsBase64String()
    {
        var text = "MockText";

        var result = await qrCodeService.GetQRCodeAsBase64String(text, 10);

        result.Should().BeOfType<string>();
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GetQRCodeAsBase64String_ReturnsEmptyString_WhenNoTextProvided()
    {
        var text = string.Empty;

        var result = await qrCodeService.GetQRCodeAsBase64String(text, 10);

        result.Should().BeOfType<string>();
        Assert.Equal(result, string.Empty);
    }

    [Fact]
    public async Task GetQRCodeAsImageUrl_ReturnsQRCodeAsImageUrl()
    {
        var text = "MockText";

        var result = await qrCodeService.GetQRCodeAsImageUrl(text, 10);

        result.Should().BeOfType<string>();
        result.Should().Contain("base64");
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GetQRCodeAsImageUrl_ReturnsEmptyString_WhenNoTextProvided()
    {
        var text = string.Empty;

        var result = await qrCodeService.GetQRCodeAsImageUrl(text, 10);

        result.Should().BeOfType<string>();
        Assert.Equal(result, string.Empty);
    }
}
