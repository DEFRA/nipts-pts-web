using Defra.PTS.Web.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.PTS.Web.Application.Extensions;
using System.ComponentModel;
using Defra.PTS.Web.Domain.Attributes;

namespace Defra.PTS.Web.Application.UnitTests.Extensions
{
    public class EnumExtensionsTests
    {
        [Theory]
        [InlineData(PetSpecies.Dog)]
        [InlineData(PetSpecies.Cat)]
        public void HasBreed_ReturnsTrue_ForSpeciesWithBreeds(PetSpecies species)
        {
            // Arrange

            // Act
            var result = species.HasBreed();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(PetSpecies.Ferret)]
        public void HasBreed_ReturnsFalse_ForSpeciesWithoutBreeds(PetSpecies species)
        {
            // Arrange

            // Act
            var result = species.HasBreed();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(PetSpecies.Ferret)]
        [InlineData(PetSpecies.Dog)]
        [InlineData(PetSpecies.Cat)]
        public void GetDescription_Returns_ForSpecies(PetSpecies species)
        {
            // Arrange

            // Act
            var result = species.GetDescription();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(species.ToString(), result);
        }

        [Theory]
        [InlineData(TestSpecies.Ferret)]
        [InlineData(TestSpecies.Dog)]
        [InlineData(TestSpecies.Cat)]
        public void GetName_Returns_ForSpecies(TestSpecies species)
        {
            // Arrange

            // Act
            var result = species.GetName();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(species.ToString(), result);
        }

        public enum TestSpecies
        {
            [Name("")]
            [Description("")]
            None = 0,

            [Name("Dog")]
            [Description("Dog")]
            Dog = 1,

            [Name("Cat")]
            [Description("Cat")]
            Cat = 2,

            [Name("Ferret")]
            [Description("Ferret")]
            Ferret = 3
        }

    }
}
