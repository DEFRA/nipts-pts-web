using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;

public class AddAddressHandler : IRequestHandler<AddAddressRequest, AddAddressResponse>
{
    private readonly IDynamicService _dynamicService;
    private readonly ILogger<AddAddressHandler> _logger;

    public AddAddressHandler(IDynamicService dynamicService, ILogger<AddAddressHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(dynamicService);
        ArgumentNullException.ThrowIfNull(logger);

        _dynamicService = dynamicService;
        _logger = logger;
    }

    public async Task<AddAddressResponse> Handle(AddAddressRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _dynamicService.AddAddressAsync(request.User);
            return new AddAddressResponse
            {
                IsSuccess = true,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(_dynamicService)}: Unable to add address for {request?.User?.UniqueReference}", ex);
            throw;
        }
    }
}
