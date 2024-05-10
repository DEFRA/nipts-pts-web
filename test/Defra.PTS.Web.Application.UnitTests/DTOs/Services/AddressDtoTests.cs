using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Services;

public class AddressDtoTests
{
    [Theory]
    [InlineData("Id")]
    [InlineData("AddressLineOne")]
    [InlineData("AddressLineTwo")]
    [InlineData("TownOrCity")]
    [InlineData("County")]
    [InlineData("PostCode")]
    [InlineData("CountryName")]
    [InlineData("AddressType")]
    [InlineData("IsActive")]
    [InlineData("CreatedBy")]
    [InlineData("CreatedOn")]
    [InlineData("UpdatedBy")]
    [InlineData("UpdatedOn")]
    public void AddressDto_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<AddressDto>(propertyName);

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
    public void AddressDto_HasCorrectData()
    {
        // Arrange
        var dt = DateTime.UtcNow;
        var model = new AddressDto
        {
            Id = Guid.Empty,
            AddressLineOne = "AddressLineOne",
            AddressLineTwo = "AddressLineTwo",
            TownOrCity = "TownOrCity",
            County = "County",
            PostCode = "PostCode",
            CountryName = "CountryName",
            AddressType = "AddressType",
            IsActive = true,
            CreatedBy = Guid.Empty,
            CreatedOn = dt,
            UpdatedBy = null,
            UpdatedOn = null
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.Id.Should().Be(Guid.Empty);
            model.AddressLineOne.Should().Be("AddressLineOne");
            model.AddressLineTwo.Should().Be("AddressLineTwo");
            model.TownOrCity.Should().Be("TownOrCity");
            model.County.Should().Be("County");
            model.PostCode.Should().Be("PostCode");
            model.CountryName.Should().Be("CountryName");
            model.AddressType.Should().Be("AddressType");
            model.IsActive.Should().Be(true);
            model.CreatedBy.Should().Be(Guid.Empty);
            model.CreatedOn.Should().Be(dt);
            model.UpdatedBy.Should().BeNull();
            model.UpdatedOn.Should().BeNull();
        }
    }
}
