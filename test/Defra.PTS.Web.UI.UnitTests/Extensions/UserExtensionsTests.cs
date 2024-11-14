using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Extensions;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Security.Claims;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Extensions;

public class UserExtensionsTests
{
    private readonly Guid contactId = Guid.NewGuid();
    private readonly Guid userId = Guid.NewGuid();

    [Fact]
    public void ToUserInfo_WhenClaimsPrincipalIsProvided_ReturnsValues()
    {
        // Arrange
        var claimsPrincipal = MockUser();

        // Act
        var userInfo = claimsPrincipal.ToUserInfo();

        // Assert
        using (new AssertionScope())
        {
            userInfo.Should().BeOfType<Domain.Models.User>();

            userInfo.FirstName.Should().Be("John");
            userInfo.LastName.Should().Be("Doe");
            userInfo.EmailAddress.Should().Be("test@test.com");
            userInfo.Role.Should().Be("test");
            userInfo.UniqueReference.Should().Be("TESTREF123");
        }
    }

    [Fact]
    public void ToUserInfo_WhenClaimsPrincipalIsNull_ThrowsException()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = null;

        // Act
        Action act = () => claimsPrincipal.ToUserInfo();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }


    [Fact]
    public void ToUserInfo_WhenClaimsIdentityIsProvided_ReturnsValues()
    {
        // Arrange
        var claimsIdentity = MockUser().Identities.First();

        // Act
        var userInfo = claimsIdentity.ToUserInfo();

        // Assert
        using (new AssertionScope())
        {
            userInfo.Should().BeOfType<Domain.Models.User>();

            userInfo.FirstName.Should().Be("John");
            userInfo.LastName.Should().Be("Doe");
            userInfo.EmailAddress.Should().Be("test@test.com");
            userInfo.Role.Should().Be("test");
            userInfo.UniqueReference.Should().Be("TESTREF123");
        }
    }

    [Fact]
    public void ToUserInfo_WhenClaimsIdentityIsNull_ThrowsException()
    {
        // Arrange
        ClaimsIdentity claimsIdentity = null;

        // Act
        Action act = () => claimsIdentity.ToUserInfo();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }

    [Fact]
    public void GetLoggedInUserId_WhenValidUser_ReturnsId()
    {
        // Arrange
        var claimsPrincipal = MockUser();

        // Act
        var result = claimsPrincipal.GetLoggedInUserId();

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(userId);    
        }
    }

    [Fact]
    public void GetLoggedInUserId_WhenClaimsPrincipalIsNull_ThorwsException()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = null;

        // Act
        Action act = () => claimsPrincipal.GetLoggedInUserId();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }

    [Fact]
    public void GetLoggedInUserId_WhenNoValidUser_ThrowsException()
    {
        // Arrange
        var claimsPrincipal = MockUser(hasUserId: false);

        // Act
        Action act = () => claimsPrincipal.GetLoggedInUserId();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<InvalidDataException>(act);
            exception.Message.Should().StartWith("Invalid userId provided");
        }
    }

    [Fact]
    public void GetLoggedInContactId_WhenValidUser_ReturnsId()
    {
        // Arrange
        var claimsPrincipal = MockUser();

        // Act
        var result = claimsPrincipal.GetLoggedInContactId();

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(contactId);
        }
    }

    [Fact]
    public void GetLoggedInContactId_WhenClaimsPrincipalIsNull_ThorwsException()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = null;

        // Act
        Action act = () => claimsPrincipal.GetLoggedInContactId();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }

    [Fact]
    public void GetLoggedInContactId_WhenNoValidUser_ThorwsException()
    {
        // Arrange
        var claimsPrincipal = MockUser(hascontactId: false);

        // Act
        Action act = () => claimsPrincipal.GetLoggedInContactId();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<InvalidDataException>(act);
            exception.Message.Should().StartWith("Invalid contactId provided");
        }
    }

    [Fact]
    public void GetLoggedInUserName_WhenValidUser_ReturnsUsername()
    {
        // Arrange
        var claimsPrincipal = MockUser();

        // Act
        var result = claimsPrincipal.GetLoggedInUserName();

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be("JohnDoe");
        }
    }

    [Fact]
    public void GetLoggedInUserName_WhenClaimsPrincipalIsNull_ThorwsException()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = null;

        // Act
        Action act = () => claimsPrincipal.GetLoggedInUserName();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }

    [Fact]
    public void GetLoggedInUserEmail_WhenValidUser_ReturnsUsername()
    {
        // Arrange
        var claimsPrincipal = MockUser();

        // Act
        var result = claimsPrincipal.GetLoggedInUserEmail();

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be("test@test.com");
        }
    }

    [Fact]
    public void GetLoggedInUserEmail_WhenClaimsPrincipalIsNull_ThorwsException()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = null;

        // Act
        Action act = () => claimsPrincipal.GetLoggedInUserEmail();

        // Assert
        using (new AssertionScope())
        {
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().StartWith("Value cannot be null.");
        }
    }


    private ClaimsPrincipal MockUser(bool hasUserId = true, bool hascontactId = true)
    {
        var claims = new List<Claim>()
        {
            new ("firstName", "John"),
            new ("lastName", "Doe"),
            new (ClaimTypes.Name, "JohnDoe"),
            new (ClaimTypes.NameIdentifier, hasUserId ? userId.ToString() : string.Empty),
            new (WebAppConstants.IdentityKeys.PTSUserId, hasUserId ? userId.ToString() : string.Empty),
            new ("contactId", hascontactId ? contactId.ToString() : string.Empty),
            new ("uniqueReference", "TESTREF123"),
            new ("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "test@test.com"),
            new ("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "test")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");

        return new ClaimsPrincipal(identity);
    }
}
