using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Extensions
{
    public class ViewModelExtensionsTests
    {
        [Fact]
        public void GetPetOwnerName_ReturnsName_FromPetKeeperName_WhenCompleted()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperName = new PetKeeperNameViewModel { IsCompleted = true, Name = "John Doe" }
            };

            // Act
            var result = dto.GetPetOwnerName();

            // Assert
            Assert.Equal("John Doe", result);
        }

        [Fact]
        public void GetPetOwnerName_ReturnsName_FromPetKeeperUserDetails_WhenPetKeeperName_NotCompleted()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel { Name = "Jane Doe" }
            };

            // Act
            var result = dto.GetPetOwnerName();

            // Assert
            Assert.Equal("Jane Doe", result);
        }


        [Fact]
        public void GetPetOwnerAddress_ReturnsManualAddress_WhenPetKeeperAddressManual_Completed()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperAddressManual = new PetKeeperAddressManualViewModel
                {
                    IsCompleted = true,
                    AddressLineOne = "123 Main St",
                    TownOrCity = "Anytown",
                    Postcode = "12345"
                }
            };

            // Act
            var result = dto.GetPetOwnerAddress();

            // Assert
            Assert.Equal("123 Main St", result.AddressLineOne);
            Assert.Equal("Anytown", result.TownOrCity);
            Assert.Equal("12345", result.PostCode);
        }

        [Fact]
        public void GetPetOwnerAddress_ReturnsAutomaticServiceAddress_WhenPetKeeperAddressManual_Completed()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperAddress = new PetKeeperAddressViewModel
                {
                    IsCompleted = true,
                    Address = ("123 Main St;123 Main St 2;;;12345"),
                    Postcode = "12345",
                }
            };

            // Act
            var result = dto.GetPetOwnerAddress();

            // Assert
            Assert.Equal("123 Main St", result.AddressLineOne);
            Assert.Equal("123 Main St 2", result.AddressLineTwo);
            Assert.Equal("12345", result.PostCode);
        }


        [Fact]
        public void GetPetOwnerPhone_ReturnsPhone_FromPetKeeperPhone_WhenCompleted()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperPhone = new PetKeeperPhoneViewModel { IsCompleted = true, Phone = "1234567890" }
            };

            // Act
            var result = dto.GetPetOwnerPhone();

            // Assert
            Assert.Equal("1234567890", result);
        }

        [Fact]
        public void GetPetOwnerPhone_ReturnsPhone_FromPetKeeperUserDetails_WhenPetKeeperPhone_NotCompleted()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel { Phone = "9876543210" }
            };

            // Act
            var result = dto.GetPetOwnerPhone();

            // Assert
            Assert.Equal("9876543210", result);
        }

        [Fact]
        public void GetPetOwnerPhone_ReturnsNullString_WhenPetKeeperPhone_IsNull_AndPetKeeperUserDetails_PhoneIsNull()
        {
            // Arrange
            var dto = new TravelDocumentViewModel();

            // Act
            var result = dto.GetPetOwnerPhone();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetPetOwnerPhone_ReturnsEmptyString_WhenPetKeeperPhone_IsNull_AndPetKeeperUserDetails_PhoneIsEmpty()
        {
            // Arrange
            var dto = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel { Phone = "" }
            };

            // Act
            var result = dto.GetPetOwnerPhone();

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
