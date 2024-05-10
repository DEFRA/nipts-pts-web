using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.Components;

public class PetDetailsCvmShould
{
    [Theory]
    [InlineData("BirthDate")]
    [InlineData("Feature")]
    [InlineData("Breed")]
    [InlineData("HasBreed")]
    [InlineData("Gender")]
    [InlineData("ShowBreed")]
    [InlineData("Species")]
    [InlineData("Name")]
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
    public void HaveValidData()
    {
        // Arrange
        var model = CreateModel();

        // Act
        model.BirthDate = "14/03/2023";
        model.Colour = "Mixed White and Tan";
        model.Breed = "Yes";
        model.HasBreed = true;
        model.Gender = "Female";
        model.ShowBreed = true;
        model.Species = "Dog";
        model.Name = "Kitsu";
        model.Feature = "Scar above eye";

        // Assert
        using (new AssertionScope())
        {
            model.BirthDate.Should().Be("14/03/2023");
            model.Colour.Should().Be("Mixed White and Tan");
            model.Breed.Should().Be("Yes");
            model.HasBreed.Should().Be(true);
            model.Gender.Should().Be("Female");
            model.ShowBreed.Should().Be(true);
            model.Species.Should().Be("Dog");
            model.Name.Should().Be("Kitsu");
            model.Feature.Should().Be("Scar above eye");
        }
    }

    #region PrivateMethods
    private static PetDetailsCvm CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetDetailsCvm>(propertyName);
    #endregion PrivateMethods
}
