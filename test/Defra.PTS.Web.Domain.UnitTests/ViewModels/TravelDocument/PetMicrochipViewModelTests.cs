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
    public class PetMicrochipViewModelTests
    {
        [Theory]
        [InlineData("FormTitle")]
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
        [InlineData("Microchipped")]
        [InlineData("MicrochipNumber")]
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
            result.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochip);
        }

        [Fact]
        public void HaveCorrectTitle()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.FormTitle;

            // Assert
            result.Should().Be($"Is your pet microchipped?");
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
            model.MicrochipNumber = "Kitsu";
            model.Microchipped = Enums.YesNoOptions.Yes;

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochip);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.MicrochipNumber = "Kitsu";
            model.Microchipped = Enums.YesNoOptions.Yes;

            // Act
            model.ClearData();

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochip);
                model.IsCompleted.Should().Be(default);
                model.MicrochipNumber.Should().Be(default);
                model.Microchipped.Should().Be(default);
            }
        }

        [Theory]
        [InlineData(Enums.YesNoOptions.Yes, "Kitsu ", "Kitsu")]
        [InlineData(Enums.YesNoOptions.Yes, null, "")]
        [InlineData(Enums.YesNoOptions.No, null, "")]
        public void VerifyTrimUnwantedDataWorksAsExpected(Enums.YesNoOptions option, string microChipNumber, string result)
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.MicrochipNumber = microChipNumber;
            model.Microchipped = option;


            // Act
            model.TrimUnwantedData();

            // Assert
            using (new AssertionScope())
            {
                Assert.True(model.IsCompleted);
                Assert.Equal(result, model.MicrochipNumber);
                Assert.Equal(option, model.Microchipped);
            }
        }


        private static PetMicrochipViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetMicrochipViewModel>(propertyName);
    }
}
