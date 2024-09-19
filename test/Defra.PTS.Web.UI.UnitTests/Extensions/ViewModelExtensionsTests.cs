using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Extensions;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System.Net.Http;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Extensions;

public class ViewModelExtensionsTests
{
    [Theory]
    [InlineData(TravelDocumentFormPageType.PetKeeperUserDetails)]
    [InlineData(TravelDocumentFormPageType.PetKeeperName)]
    [InlineData(TravelDocumentFormPageType.PetKeeperPostcode)]
    [InlineData(TravelDocumentFormPageType.PetKeeperAddress)]
    [InlineData(TravelDocumentFormPageType.PetKeeperAddressManual)]
    [InlineData(TravelDocumentFormPageType.PetKeeperPhone)]
    [InlineData(TravelDocumentFormPageType.PetMicrochip)]
    [InlineData(TravelDocumentFormPageType.PetMicrochipNotAvailable)]
    [InlineData(TravelDocumentFormPageType.PetMicrochipDate)]
    [InlineData(TravelDocumentFormPageType.PetSpecies)]
    [InlineData(TravelDocumentFormPageType.PetBreed)]
    [InlineData(TravelDocumentFormPageType.PetName)]
    [InlineData(TravelDocumentFormPageType.PetGender)]
    [InlineData(TravelDocumentFormPageType.PetAge)]
    [InlineData(TravelDocumentFormPageType.PetColour)]
    [InlineData(TravelDocumentFormPageType.PetFeature)]
    [InlineData(TravelDocumentFormPageType.Declaration)]
    [InlineData(TravelDocumentFormPageType.Acknowledgement)]
    public void DoesPageMeetPreConditions_WhenAllPreConditionsAreSatisfied(TravelDocumentFormPageType pageType)
    {
        // Arrange
        var viewModel = GetCompletedModel(true);

        // Act
        var result = viewModel.DoesPageMeetPreConditions(pageType, out string actionName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetKeeperUserDetailsIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetMicrochip, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetKeeperUserDetails");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetKeeperNameIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
        viewModel.PetKeeperName.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetKeeperPostcode, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetKeeperName");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetKeeperPostcodeIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
        viewModel.PetKeeperPostcode.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetKeeperAddress, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetKeeperPostcode");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenCompletedIsTrue_WhenPetKeeperPostcodeIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
        viewModel.PetKeeperPostcode.IsCompleted = true;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetKeeperAddress, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetKeeperAddressorManualAddressIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
        viewModel.PetKeeperPostcode.IsCompleted = false;
        viewModel.PetKeeperAddress.IsCompleted = false;
        viewModel.PetKeeperAddressManual.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetKeeperPhone, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetKeeperPostcode");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetKeeperPhoneIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetKeeperUserDetails.UserDetailsAreCorrect = YesNoOptions.No;
        viewModel.PetKeeperPhone.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetMicrochip, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetKeeperPhone");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetMicrochipIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetMicrochip.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetMicrochipDate, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetMicrochip");
        }
    }


    [Fact]
    public void DoesPageMeetPreConditions_WhenPetMicrochipDateIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetMicrochipDate.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetSpecies, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetMicrochipDate");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetSpeciesIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetSpecies.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetBreed, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetSpecies");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetBreedIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetBreed.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetName, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetBreed");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetNameIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetName.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetGender, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetName");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetGenderIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetGender.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetAge, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetGender");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetAgeIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetAge.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetColour, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetAge");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetColourIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetColour.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.PetFeature, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetColour");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenPetFeatureIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.PetFeature.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.Declaration, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("PetFeature");
        }
    }

    [Fact]
    public void DoesPageMeetPreConditions_WhenDeclarationIsRequired()
    {
        // Arrange
        var viewModel = GetCompletedModel(true);
        viewModel.Declaration.IsCompleted = false;

        // Act
        var result = viewModel.DoesPageMeetPreConditions(TravelDocumentFormPageType.Acknowledgement, out string actionName);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeFalse();
            actionName.Should().Be("Declaration");
        }
    }

    #region Private Methods
    private static TravelDocumentViewModel GetEmptyModel()
    {
        return new();
    }

    private TravelDocumentViewModel GetCompletedModel(bool isCompleted = true)
    {
        return new()
        {
            RequestId = Guid.NewGuid(),
            IsApplicationInProgress = true,
            IsSubmitted = isCompleted,

            PetKeeperUserDetails = GetPetKeeperUserDetails(isCompleted),

            PetKeeperName = GetPetKeeperName(isCompleted),
            PetKeeperPostcode = GetPetKeeperPostcode(isCompleted),
            PetKeeperAddress = GetPetKeeperAddress(isCompleted),
            PetKeeperAddressManual = GetPetKeeperAddressManual(isCompleted),
            PetKeeperPhone = GetPetKeeperPhone(isCompleted),

            PetMicrochip = GetPetMicrochip(isCompleted),
            PetMicrochipDate = GetPetMicrochipDate(isCompleted),
            PetMicrochipNotAvailable = GetPetMicrochipNotAvailable(isCompleted),

            PetAge = GetPetAge(isCompleted),
            PetSpecies = GetPetSpecies(isCompleted),
            PetBreed = GetPetBreed(isCompleted),
            PetColour = GetPetColour(isCompleted),
            PetGender = GetPetGender(isCompleted),
            PetName = GetPetName(isCompleted),
            PetFeature = GetPetFeature(isCompleted),

            Declaration = GetDeclaration(isCompleted),
            Acknowledgement = GetAcknowledgement(isCompleted),
        };

    }

    private static PetMicrochipNotAvailableViewModel GetPetMicrochipNotAvailable(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
        };
    }


    private static PetMicrochipDateViewModel GetPetMicrochipDate(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Day = "1",
            Month = "1",
            Year = "2024",
            BirthDate = new DateTime(2020, 1, 1)
        };
    }

    private static PetMicrochipViewModel GetPetMicrochip(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            MicrochipNumber = "123456789012345",
            Microchipped = Domain.Enums.YesNoOptions.Yes
        };
    }

    private static PetKeeperPhoneViewModel GetPetKeeperPhone(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Phone = "07777777777"
        };
    }

    private static PetKeeperAddressManualViewModel GetPetKeeperAddressManual(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            AddressLineOne = "Address Line 1",
            Postcode = "G2 4SQ",
            PostcodeRegion = Domain.Enums.PostcodeRegion.GB
        };
    }

    private static PetKeeperAddressViewModel GetPetKeeperAddress(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Address = "Address line 1, Glasgow, Scotland, G2 4SQ",
            PostcodeRegion = Domain.Enums.PostcodeRegion.GB
        };
    }

    private static PetKeeperPostcodeViewModel GetPetKeeperPostcode(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Postcode = "G2 4SQ",
            PostcodeRegion = Domain.Enums.PostcodeRegion.GB
        };
    }


    private static PetKeeperNameViewModel GetPetKeeperName(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Name = "Test"
        };
    }

    private static PetKeeperUserDetailsViewModel GetPetKeeperUserDetails(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            UserDetailsAreCorrect = Domain.Enums.YesNoOptions.Yes,
            AddressLineOne = "Address Line 1",
            Name = "Test",
            Email = "test@test.com",
            Phone = "07111111111",
            Postcode = "G2 4SQ",
            PostcodeRegion = Domain.Enums.PostcodeRegion.GB
        };
    }

    private static PetSpeciesViewModel GetPetSpecies(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            PetSpecies = Domain.Enums.PetSpecies.Dog
        };
    }

    private static PetNameViewModel GetPetName(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            PetName = "Test"
        };
    }

    private static PetGenderViewModel GetPetGender(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Gender = Domain.Enums.PetGender.Male
        };
    }


    private static PetFeatureViewModel GetPetFeature(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            FeatureDescription = "Test",
            HasUniqueFeature = Domain.Enums.YesNoOptions.Yes,
        };
    }

    private static PetColourViewModel GetPetColour(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            PetSpecies = Domain.Enums.PetSpecies.Dog,
            OtherColourID = 0,
            PetColour = 1,
            PetColourName = "Test",
            PetColourOther = string.Empty
        };
    }

    private static PetBreedViewModel GetPetBreed(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            BreedId = 1,
            BreedName = "Test",
            BreedAdditionalInfo = string.Empty,
            PetSpecies = Domain.Enums.PetSpecies.Dog
        };
    }

    private static PetAgeViewModel GetPetAge(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            Day = 1.ToString(),
            Month = 1.ToString(),
            Year = 2020.ToString(),
            MicrochippedDate = new DateTime(2024, 1, 1),
        };
    }

    private static AcknowledgementViewModel GetAcknowledgement(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            IsSuccess = true,
            Reference = "ABCXYZ"
        };
    }

    private static DeclarationViewModel GetDeclaration(bool isCompleted = true)
    {
        return new()
        {
            IsCompleted = isCompleted,
            IsManualAddress = false,
            Phone = "07777777777",
            Postcode = "G2 4SQ",
            RequestId = Guid.NewGuid(),
            AgreedToAccuracy = true,
            AgreedToDeclaration = true,
            AgreedToPrivacyPolicy = true
        };
    }
    #endregion PrivateMethods
}
