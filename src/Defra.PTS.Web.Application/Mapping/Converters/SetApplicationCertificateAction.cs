using AutoMapper;
using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.QRCoder.Services.Interfaces;

namespace Defra.PTS.Web.Application.Mapping.Converters;

public class SetApplicationCertificateAction : IMappingAction<VwApplication, ApplicationCertificateDto>
{
    private readonly IQRCodeService _qrCodeService;

    public SetApplicationCertificateAction(IQRCodeService qrCodeService)
    {
        ArgumentNullException.ThrowIfNull(qrCodeService);
        _qrCodeService = qrCodeService;
    }

    public void Process(VwApplication source, ApplicationCertificateDto destination, ResolutionContext context)
    {
        var referenceNumberQrCode = _qrCodeService
            .GetQRCodeAsBase64String(destination.ReferenceNumber, AppConstants.Values.QRCodePixelsPerModule)
            .GetAwaiter()
            .GetResult();

        var documentReferenceNumberQrCode = _qrCodeService
            .GetQRCodeAsBase64String(destination.CertificateIssued.DocumentReferenceNumber, AppConstants.Values.QRCodePixelsPerModule)
            .GetAwaiter()
            .GetResult();

        destination.ReferenceNumberAsQRCode = referenceNumberQrCode;
        destination.CertificateIssued.DocumentReferenceNumberAsQRCode = documentReferenceNumberQrCode;
    }
}
