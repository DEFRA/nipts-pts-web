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
    public class PetKeeperPostcodeViewModelTests
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
            result.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperPostcode);
        }

        [Fact]
        public void Have_PetKeeperPostCodeDetails_CorrectTitle()
        {
            // Act
            var result = PetKeeperPostcodeViewModel.FormTitle;

            // Assert
            result.Should().Be($"What is your postcode?");
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
            model.Postcode = "TM64GH";
            model.PostcodeRegion = PostcodeRegion.Unknown;


            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperPostcode);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Postcode = "TM64GH";
            model.PostcodeRegion = PostcodeRegion.Unknown;

            // Act
            model.ClearData();

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperPostcode);
                model.IsCompleted.Should().Be(default);
                model.Postcode.Should().Be(default);
            }
        }

        [Fact]
        public void VerifyTrimUnwantedDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Postcode = "TM64GH";
            model.PostcodeRegion = PostcodeRegion.Unknown;


            // Act
            model.TrimUnwantedData();

            Assert.True(model.IsCompleted);
        }

        private static PetKeeperPostcodeViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetKeeperPostcodeViewModel>(propertyName);
    }
}
