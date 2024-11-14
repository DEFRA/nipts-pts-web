using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;

public class AddAddressHandler : IRequestHandler<AddAddressRequest, AddAddressResponse>
{
    private readonly IDynamicService _dynamicService;

    public AddAddressHandler(IDynamicService dynamicService)
    {
        ArgumentNullException.ThrowIfNull(dynamicService);

        _dynamicService = dynamicService;
    }

    public async Task<AddAddressResponse> Handle(AddAddressRequest request, CancellationToken cancellationToken)
    {
        await _dynamicService.AddAddressAsync(request.User);
        return new AddAddressResponse
        {
            IsSuccess = true,
        };
    }
}
