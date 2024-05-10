using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class AddressLookupResponseTests
{
    [Theory]
    [InlineData("Postcode")]
    [InlineData("Addresses")]
    public void AddressLookupResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<AddressLookupResponse>(propertyName);

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
    public void AddressLookupResponse_HasCorrectData()
    {
        // Arrange
        var model = new AddressLookupResponse
        {
            Postcode = "A12 3YZ",
            Addresses = new List<Domain.Models.Address> { new() { AddressLineOne = "Test", } }
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.Postcode.Should().Be("A12 3YZ");
            model.Addresses.Count.Should().Be(1);
        }
    }
}
