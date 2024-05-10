using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Services;

public class UserDtoTests
{
    [Theory]
    [InlineData("Id")]
    [InlineData("FullName")]
    [InlineData("Email")]
    [InlineData("FirstName")]
    [InlineData("LastName")]
    [InlineData("Role")]
    [InlineData("Telephone")]
    [InlineData("ContactId")]
    [InlineData("Uniquereference")]
    [InlineData("SignInDateTime")]
    [InlineData("SignOutDateTime")]
    [InlineData("Address")]
    [InlineData("CreatedBy")]
    [InlineData("CreatedOn")]
    [InlineData("UpdatedBy")]
    [InlineData("UpdatedOn")]
    public void UserDto_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<UserDto>(propertyName);

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
    public void UserDto_HasCorrectData()
    {
        // Arrange
        var dt = DateTime.UtcNow;
        var model = new UserDto
        {
            Id = Guid.Empty,
            FullName = "FullName",
            Email = "Email",
            FirstName = "FirstName",
            LastName = "LastName",
            Role = "Role",
            Telephone = "Telephone",
            ContactId = Guid.Empty,
            Uniquereference = "ABC123XYZ",
            SignInDateTime = dt,
            SignOutDateTime = dt.AddDays(1),
            Address = new AddressDto(),
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
            model.FullName.Should().Be("FullName");
            model.Email.Should().Be("Email");
            model.FirstName.Should().Be("FirstName");
            model.LastName.Should().Be("LastName");
            model.Role.Should().Be("Role");
            model.Telephone.Should().Be("Telephone");
            model.ContactId.Should().Be(Guid.Empty);
            model.Uniquereference.Should().Be("ABC123XYZ");
            model.SignInDateTime.Should().Be(dt);
            model.SignOutDateTime.Should().Be(dt.AddDays(1));
            model.Address.Should().NotBeNull();
            model.CreatedBy.Should().Be(Guid.Empty);
            model.CreatedOn.Should().Be(dt);
            model.UpdatedBy.Should().BeNull();
            model.UpdatedOn.Should().BeNull();
        }
    }
}
