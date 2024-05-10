using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions.Execution;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument
{
    public class PetMicrochipNotAvailableViewModelTests
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

        [Fact]
        public void HaveCorrectTitle()
        {
            // Arrange
            var model = CreateModel();

            // Act
            var result = model.FormTitle;

            // Assert
            result.Should().Be($"Get your pet microchipped before applying");
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

            // Assert
            using (new AssertionScope())
            {
                model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetMicrochipNotAvailable);
                model.IsCompleted.Should().Be(true);
            }
        }

        [Fact]
        public void VerifyClearDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;


            // Act
            model.ClearData();

            Assert.True(model.IsCompleted);
        }

        [Fact]
        public void VerifyTrimUnwantedDataWorksAsExpected()
        {
            // Arrange
            var model = CreateModel();

            model.IsCompleted = true;


            // Act
            model.TrimUnwantedData();

            Assert.True(model.IsCompleted);
        }

        private static PetMicrochipNotAvailableViewModel CreateModel() => new();
        private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetMicrochipNotAvailableViewModel>(propertyName);
    }
}
