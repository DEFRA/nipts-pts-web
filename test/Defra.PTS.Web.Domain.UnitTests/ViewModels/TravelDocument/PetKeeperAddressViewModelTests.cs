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
    public class PetKeeperAddressViewModelTests
    {
        [Theory]
        [InlineData("PageType")]
        [InlineData("FormTitle")]
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
        [InlineData("Address")]
        [InlineData("Postcode")]
        [InlineData("PostcodeRegion")]
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
            result.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddress);
        }

        [Fact]
        public void Have_PetKeeperPostCodeDetails_CorrectTitle()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.FormTitle;

            // Assert
            result.Should().Be($"What is your address?");
        }

        [Fact]
        public void BeAssignableToTravelDocumentFormPage()
        {
            // Arrange
            var model = CreateModel();

            // Assert
            model.Should().BeAssignableTo<Domain.ViewModels.TravelDocument.TravelDocumentFormPage>();
        }

        [Fact]
        public void HaveValidData()
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.IsCompleted = true;
            model.Address = "NG Test Address";
            model.Postcode = "RM6 4FB";
            model.PostcodeRegion = PostcodeRegion.Unknown;

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddress);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Address = "NG Test Address";
            model.Postcode = "RM6 4FB";
            model.PostcodeRegion = PostcodeRegion.Unknown;

            // Act
            model.ClearData();

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddress);
                model.IsCompleted.Should().Be(default);
                model.Address.Should().Be(default);
                model.Postcode.Should().Be(default);
            }
        }

        [Fact]
        public void VerifyTrimUnwantedDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Address = "NG Test Address";
            model.Postcode = "RM6 4FB";
            model.PostcodeRegion = PostcodeRegion.Unknown;

            // Act
            model.TrimUnwantedData();

            Assert.True(model.IsCompleted);
        }

        private static PetKeeperAddressViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetKeeperAddressViewModel>(propertyName);
    }
}
