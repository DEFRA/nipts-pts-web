using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.Components;

public class PetKeeperDetailsCvmShould
{
    [Theory]
    [InlineData("Address")]
    [InlineData("AddressUrl")]
    [InlineData("Name")]
    [InlineData("NameUrl")]
    [InlineData("Email")]
    [InlineData("Phone")]
    [InlineData("PhoneUrl")]
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

        var address = new Models.Address
        {
            AddressLineOne = "11 Test Lane",
            AddressLineTwo = "User Town",
            County = "AC County",
            Postcode = "ID1 OT",
            TownOrCity = "User City"
        };

        // Act
        model.AddressUrl = "/Address/test";
        model.Name = "Test User";
        model.NameUrl = "/Name/test";
        model.Email = "user@test.com";
        model.Phone = "+447302442001";
        model.PhoneUrl = "/Phone/test";
        model.Address = address;

        // Assert
        using (new AssertionScope())
        {
            model.AddressUrl.Should().Be("/Address/test");
            model.Name.Should().Be("Test User");
            model.NameUrl.Should().Be("/Name/test");
            model.Email.Should().Be("user@test.com");
            model.Phone.Should().Be("+447302442001");
            model.PhoneUrl.Should().Be("/Phone/test");
            model.Address.Should().Be(address);
        }
    }

    #region PrivateMethods
    private static PetKeeperDetailsCvm CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetKeeperDetailsCvm>(propertyName);
    #endregion PrivateMethods
}
