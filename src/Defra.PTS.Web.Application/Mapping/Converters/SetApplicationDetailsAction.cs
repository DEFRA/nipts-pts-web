using AutoMapper;
using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.QRCoder.Services.Interfaces;

namespace Defra.PTS.Web.Application.Mapping.Converters;

public class SetApplicationDetailsAction : IMappingAction<VwApplication, ApplicationDetailsDto>
{
    private readonly IQRCodeService _qrCodeService;

    public SetApplicationDetailsAction(IQRCodeService qrCodeService)
    {
        ArgumentNullException.ThrowIfNull(qrCodeService);
        _qrCodeService = qrCodeService;
    }

    public void Process(VwApplication source, ApplicationDetailsDto destination, ResolutionContext context)
    {
        var referenceNumberQrCode = _qrCodeService
            .GetQRCodeAsBase64String(destination.ReferenceNumber, AppConstants.Values.QRCodePixelsPerModule)
            .GetAwaiter()
            .GetResult();

        destination.ReferenceNumberAsQRCode = referenceNumberQrCode;
    }
}

