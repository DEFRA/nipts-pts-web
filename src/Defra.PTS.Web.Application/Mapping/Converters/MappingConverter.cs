﻿using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using System.Linq;

namespace Defra.PTS.Web.Application.Mapping.Converters;

public static class MappingConverter
{
    public static CertificateIssuedDto MapCertificateIssued(VwApplication src)
    {
        return new CertificateIssuedDto
        {
            DocumentIssueDate = src.DocumentIssueDate,
            DocumentReferenceNumber = src.DocumentReferenceNumber,
        };
    }

    public static CertificateIssuingAuthorityDto MapCertificateIssuingAuthority(VwApplication src)
    {
        return new CertificateIssuingAuthorityDto
        {
            AuthorityName = "Animal and Plant Health Agency",

            AddressLine1 = "Pet Travel Section",
            AddressLine2 = "Eden Bridge House",
            AddressLine3 = "Lowther Street",
            TownOrCity = "Carlisle",
            Postcode = "CA3 8DX",

            SignedOnBehalfOfAPHA = src.DocumentSignedBy
        };
    }

    public static MicrochipInformationDto MapMicrochipInformation(VwApplication src)
    {
        return new MicrochipInformationDto
        {
            MicrochipDate = src.MicrochippedDate.GetValueOrDefault(),
            MicrochipNumber = src.MicrochipNumber,
            // we need to display the microchip location as "Under the skin" on the pet travel document, even though we don't ask the user the question.
            // This is from Policy as it is a requirement for the field to be on the pet travel document.
            MicrochipImplantLocation = "Under the skin"
        };
    }

    public static DeclarationDto MapDeclaration(VwApplication src)
    {
        return new DeclarationDto
        {
            ApplicationDate = src.DateOfApplication,
            PetOwnerName = src.OwnerName
        };
    }

    public static PetDetailsDto MapPetDetails(VwApplication src)
    {
        var genderName = string.Empty;
        var speciesName = string.Empty;
        var breedName = src.PetBreedName ?? string.Empty;
        var colourName = src.PetColourName ?? string.Empty;

        var hasBreed = false;

        if (Enum.TryParse(src.PetGenderId.ToString(), true, out PetGender gender))
        {
            genderName = gender.GetDescription();
        }

        if (Enum.TryParse(src.PetSpeciesId.ToString(), true, out PetSpecies species))
        {
            speciesName = species.GetDescription();
            hasBreed = species.HasBreed();
        }

        // Cat/Dog : Mixed breed or unknown
        if (breedName.StartsWith("Mixed") && !string.IsNullOrWhiteSpace(src.PetBreedOther))
        {
            breedName = src.PetBreedOther;
        }

        // Other Colour
        if (colourName.ToLowerInvariant() == AppConstants.Values.OtherColourName.ToLowerInvariant() 
            && !string.IsNullOrWhiteSpace(src.PetColourOther))
        {
            colourName = src.PetColourOther;
        }

        return new PetDetailsDto
        {
            Name = src.PetName,
            BirthDate = src.PetBirthDate.GetValueOrDefault(),
            Breed = breedName,
            Colour = colourName,
            Feature = src.PetHasUniqueFeature.GetValueOrDefault() == 1 ? src.PetUniqueFeature : "No",
            Gender = genderName,
            Species = speciesName,
            HasBreed = hasBreed
        };
    }

    public static PetKeeperDetailsDto MapPetKeeperDetails(VwApplication src)
    {
        return new PetKeeperDetailsDto
        {
            Name = src.OwnerName,
            Email = src.OwnerEmail,
            Phone = src.OwnerPhoneNumber,
            AddressLine1 = src.OwnerAddressLineOne,
            AddressLine2 = src.OwnerAddressLineTwo,
            AddressLine3 = src.OwnerCounty,
            Postcode = src.OwnerPostcode,
            TownOrCity = src.OwnerTownOrCity
        };
    }
}
