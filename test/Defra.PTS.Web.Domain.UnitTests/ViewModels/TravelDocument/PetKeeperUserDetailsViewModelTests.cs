using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument
{
    public class PetKeeperUserDetailsViewModelTests
    {
        [Theory]
        [InlineData("PetKeeperUserDetailsFormTitle")]
        [InlineData("PetKeeperNonGbAddressFormTitle")]
        [InlineData("PetKeeperNonGbAddressFormMessageHeader")]
        [InlineData("PetKeeperNonGbAddressFormMessageBody")]
        [InlineData("PageType")]        
        public void HavePropertiesWithOnlyGetters(string propertyName)
        {
            // Arrange
            var property = GetPropertyInfo(propertyName);

            // Act

            // Assert
            using (new AssertionScope())
            {
                property.Should().NotBeWritable();
                property.Should().BeReadable();
            }
        }

        [Theory]
        [InlineData("IsCompleted")]
        [InlineData("Name")]
        [InlineData("Phone")]
        [InlineData("Email")]
        [InlineData("AddressLineOne")]
        [InlineData("TownOrCity")]
        [InlineData("Postcode")]
        [InlineData("UserDetailsAreCorrect")]
        [InlineData("PostcodeRegion")]
        [InlineData("PetOwnerDetailsRequired")]
        public void HavePropertiesWithGettersAndSetters(string propertyName)
        {
            // Arrange
            var property = GetPropertyInfo(propertyName);

            // Act

            // Assert
            using (new AssertionScope())
            {
                property.Should().BeWritable();
                property.Should().BeReadable();
            }
        }

        [Fact]
        public void HaveCorrectPageType()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.PageType;

            // Assert
            result.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperUserDetails);
        }

        [Fact]
        public void Have_PetKeeperUserDetails_CorrectTitle()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.PetKeeperUserDetailsFormTitle;

            // Assert
            result.Should().Be($"Are your details correct?");
        }

        [Fact]
        public void Have_PetKeeperNonGbAddress_CorrectTitle()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.PetKeeperNonGbAddressFormTitle;

            // Assert
            result.Should().Be($"Change your details");
        }

        [Fact]
        public void Have_PetKeeperNonGbAddressForm_MessageHeader()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.PetKeeperNonGbAddressFormMessageHeader;

            // Assert
            result.Should().Be($"Important");
        }

        [Fact]
        public void Have_PetKeeperNonGbAddressForm_MessageBody()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.PetKeeperNonGbAddressFormMessageBody;

            // Assert
            result.Should().Be($"Enter an address in England, Scotland or Wales.");
        }

        [Fact]
        public void BeAssignableToTravelDocumentFormPage()
        {
            // Arrange
            var model = CreateModel();

            // Assert
            model.Should().BeAssignableTo<Domain.ViewModels.TravelDocument.TravelDocumentFormPage>();
        }

        [Theory]
        [InlineData(YesNoOptions.Yes, false)]
        [InlineData(YesNoOptions.No, true)]
        public void HaveValidData(YesNoOptions yesNoOption, bool petOwnerDetailsRequiredResult)
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.IsCompleted = true;
            model.Name = "Ninja";
            model.Phone = "7665765";
            model.Email = "n.k@gma.com";
            model.AddressLineOne = "1 AddressLineOne";
            model.TownOrCity = "1 TownOrCity";
            model.Postcode = "TM64GH";
            model.UserDetailsAreCorrect = yesNoOption;
            model.PostcodeRegion = PostcodeRegion.Unknown;


            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperUserDetails);
                model.PetOwnerDetailsRequired.Should().Be(petOwnerDetailsRequiredResult);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Name = "Ninja";
            model.Phone = "7665765";
            model.Email = "n.k@gma.com";
            model.AddressLineOne = "1 AddressLineOne";
            model.TownOrCity = "1 TownOrCity";
            model.Postcode = "TM64GH";
            model.UserDetailsAreCorrect = YesNoOptions.Yes;
            model.PostcodeRegion = PostcodeRegion.Unknown;

            // Act
            model.ClearData();

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperUserDetails);
                model.IsCompleted.Should().Be(default);
                model.Name.Should().Be(default);
                model.Phone.Should().Be(default);
                model.Email.Should().Be(default);
                model.AddressLineOne.Should().Be(default);
                model.TownOrCity.Should().Be(default);
                model.Postcode.Should().Be(default);
            }
        }

        [Fact]
        public void VerifyTrimUnwantedDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Name = "Ninja";
            model.Phone = "7665765";
            model.Email = "n.k@gma.com";
            model.AddressLineOne = "1 AddressLineOne";
            model.TownOrCity = "1 TownOrCity";
            model.Postcode = "TM64GH";
            model.UserDetailsAreCorrect = YesNoOptions.Yes;
            model.PostcodeRegion = PostcodeRegion.Unknown;


            // Act
            model.TrimUnwantedData();

            Assert.True(model.IsCompleted);
        }


        private static PetKeeperUserDetailsViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetKeeperUserDetailsViewModel>(propertyName);
    }
}
