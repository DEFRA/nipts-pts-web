using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetColoursQueryResponseTests
{
    [Theory]
    [InlineData("PetType")]
    [InlineData("Colours")]
    public void GetColoursQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetColoursQueryResponse>(propertyName);

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
    public void GetColoursQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetColoursQueryResponse
        {
            PetType = Domain.Enums.PetSpecies.Dog,
            Colours = new List<Domain.DTOs.ColourDto>()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.PetType.Should().Be(Domain.Enums.PetSpecies.Dog);
            model.Colours.Should().NotBeNull();
        }
    }
}
