using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetBreedsQueryResponseTests
{
    [Theory]
    [InlineData("PetType")]
    [InlineData("Breeds")]
    public void GetBreedsQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetBreedsQueryResponse>(propertyName);

        // Act

        // Assert
        using (new AssertionScope())
        {
            property.Should().NotBeNull();
            property.Should().BeWritable();
            property.Should().BeReadable();
        }
    }

    [Fact]
    public void GetBreedsQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetBreedsQueryResponse
        {
            PetType = Domain.Enums.PetSpecies.Dog,
            Breeds = new List<Application.DTOs.Services.BreedDto>()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.PetType.Should().Be(Domain.Enums.PetSpecies.Dog);
            model.Breeds.Should().NotBeNull();
        }
    }
}
