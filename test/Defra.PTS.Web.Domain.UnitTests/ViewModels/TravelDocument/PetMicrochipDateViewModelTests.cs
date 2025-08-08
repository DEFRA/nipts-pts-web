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
    public class PetMicrochipDateViewModelTests
    {
        [Theory]
        [InlineData("FormTitle")]
        [InlineData("PageType")]
        [InlineData("MicrochippedDate")]
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
        [InlineData("BirthDate")]
        [InlineData("Day")]
        [InlineData("Month")]
        [InlineData("Year")]
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
            result.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochipDate);
        }

        [Fact]
        public void HaveCorrectTitle()
        {
            // Act
            var result = PetMicrochipDateViewModel.FormTitle;

            // Assert
            result.Should().Be($"When was your pet microchipped or last scanned?");
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
            model.Day = "1";
            model.Month = "12";
            model.Year = "2020";
            model.BirthDate = DateTime.Now.AddYears(-5);

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochipDate);
                model.MicrochippedDate.Should().Be(new DateTime(2020, 12, 1));
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void HaveInValidData()
        {
            // Arrange
            var model = CreateModel();

            // Act
            model.IsCompleted = true;
            model.Day = "0";
            model.Month = "12";
            model.Year = "2020";
            model.BirthDate = DateTime.Now.AddYears(-5);

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochipDate);
                model.MicrochippedDate.Should().Be(null);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Day = "1";
            model.Month = "12";
            model.Year = "2020";
            model.BirthDate = DateTime.Now.AddYears(-5);

            // Act
            model.ClearData();

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochipDate);
                model.IsCompleted.Should().Be(default);
                model.Day.Should().Be(null);
                model.Month.Should().Be(null);
                model.Year.Should().Be(null);
            }
        }

        [Fact]
        public void VerifyTrimUnwantedDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;
            model.Day = "1";
            model.Month = "12";
            model.Year = "2020";
            model.BirthDate = DateTime.Now.AddYears(-5);


            // Act
            model.TrimUnwantedData();

            Assert.True(model.IsCompleted);
        }

        private static PetMicrochipDateViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetMicrochipDateViewModel>(propertyName);
    }
}
