using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Application.Extensions;

public static class ViewModelExtensions
{
    public static string GetPetOwnerName(this TravelDocumentViewModel dto)
    {
        // Pet Owner Name was added at any stage 
        if (dto.PetKeeperName.IsCompleted && !string.IsNullOrEmpty(dto.PetKeeperName.Name))
        {
            return dto.PetKeeperName.Name;
        }

        // Return from user details
        return dto.PetKeeperUserDetails.Name;
    }

    public static string GetPetOwnerPhone(this TravelDocumentViewModel dto)
    {
        // Pet Owner Phone was added at any stage 
        if (dto.PetKeeperPhone.IsCompleted && !string.IsNullOrEmpty(dto.PetKeeperPhone.Phone))
        {
            return dto.PetKeeperPhone.Phone;
        }

        // Return from user details
        return dto.PetKeeperUserDetails.Phone;
    }

    public static AddressDto GetPetOwnerAddress(this TravelDocumentViewModel dto)
    {
        // If Pet Owner Manual Address was added at any stage
        if (dto.PetKeeperAddressManual.IsCompleted)
        {
            return new AddressDto
            {
                AddressType = AddressType.Owner.ToString(),
                IsActive = true,
                AddressLineOne = dto.PetKeeperAddressManual.AddressLineOne,
                AddressLineTwo = dto.PetKeeperAddressManual.AddressLineTwo,
                County = dto.PetKeeperAddressManual.County,
                CountryName = string.Empty,
                TownOrCity = dto.PetKeeperAddressManual.TownOrCity,
                PostCode = dto.PetKeeperAddressManual.Postcode
            };
        }

        // If Pet Owner Address was selected at any stage
        if (dto.PetKeeperAddress.IsCompleted)
        {
            var address = new Address(dto.PetKeeperAddress.Address);
            return new AddressDto
            {
                AddressType = AddressType.Owner.ToString(),
                IsActive = true,
                AddressLineOne = address.AddressLineOne,
                AddressLineTwo = address.AddressLineTwo,
                County = address.County,
                CountryName = string.Empty,
                TownOrCity = address.TownOrCity,
                PostCode = address.Postcode
            };
        }

        // Get from user details
        return new AddressDto
        {
            AddressType = AddressType.Owner.ToString(),
            IsActive = true,
            AddressLineOne = dto.PetKeeperUserDetails.AddressLineOne,
            AddressLineTwo = string.Empty,
            County = string.Empty,
            CountryName = string.Empty,
            TownOrCity = dto.PetKeeperUserDetails.TownOrCity,
            PostCode = dto.PetKeeperUserDetails.Postcode
        };
    }
}
