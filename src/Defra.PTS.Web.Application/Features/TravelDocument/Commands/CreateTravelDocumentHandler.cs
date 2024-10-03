using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Commands;

public class CreateTravelDocumentHandler : IRequestHandler<CreateTravelDocumentRequest, CreateTravelDocumentResponse>
{
    private readonly IApplicationService _applicationService;
    private readonly IPetService _petService;
    private readonly IUserService _userService;
    private readonly IDynamicService _dynamicService;
    private readonly ILogger<CreateTravelDocumentHandler> _logger;

    public CreateTravelDocumentHandler(IApplicationService applicationService, IPetService petService, IUserService userService, IDynamicService dynamicService, ILogger<CreateTravelDocumentHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(petService);
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(dynamicService);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _petService = petService;
        _userService = userService;
        _dynamicService = dynamicService;
        _logger = logger;
    }

    public async Task<CreateTravelDocumentResponse> Handle(CreateTravelDocumentRequest request, CancellationToken cancellationToken)
    {
        // Get UserId
        var userId = await _userService.UpdateUserAsync(request.User.EmailAddress);

        // Pet Owner
        var ownerId = await _userService.AddOwnerAsync(request.User, request.Model);

        var ownerAddressId = await _userService.AddAddressAsync(AddressType.Owner, request.Model);

        // Create Pet 
        var petId = await _petService.CreatePet(request.Model);

        // Create Application 
        var application = new ApplicationDto
        {
            PetId = petId,
            UserId = userId,
            OwnerId = ownerId,
            OwnerNewName = request.Model.GetPetOwnerName(),
            OwnerNewTelephone = request.Model.GetPetOwnerPhone(),
            OwnerAddressId = ownerAddressId,
            IsPrivacyPolicyAgreed = request.Model.Declaration.AgreedToPrivacyPolicy,
            IsDeclarationSigned = request.Model.Declaration.AgreedToDeclaration,
            IsConsentAgreed = request.Model.Declaration.AgreedToAccuracy,
            Status = AppConstants.ApplicationStatus.AWAITINGVERIFICATION.ToUpperInvariant(),
            DateOfApplication = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        };

        Guid appId;
        CreateTravelDocumentResponse response;
        try
        {
           var  applicationResponse = await _applicationService.CreateApplication(application);
            appId = applicationResponse.Id;
            response = new CreateTravelDocumentResponse
            {
                IsSuccess = true,
                Reference = applicationResponse.ReferenceNumber,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured: {message}", e.Message);
            throw;
        }

        try
        {
            var message = new ApplicationSubmittedMessage { ApplicationId = appId };
            await _dynamicService.AddApplicationToQueueAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to send application submitted message, message: {Message}", ex.Message);
            throw;
        }

        return response;
    }
}
