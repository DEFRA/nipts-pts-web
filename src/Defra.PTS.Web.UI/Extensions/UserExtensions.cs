using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.UI.Constants;
using System.Security.Claims;

namespace Defra.PTS.Web.UI.Extensions;

public static class UserExtensions
{
    public static User ToUserInfo(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var claims = principal.Claims;

        var user = new User();
        foreach (var claim in claims)
        {
            switch (claim.Type)
            {
                case "contactId":
                    user.ContactId = claim.Value; 
                    break;
                case "uniqueReference":
                    user.UniqueReference = claim.Value; 
                    break;
                case "firstName":
                    user.FirstName = claim.Value; 
                    break;
                case "lastName":
                    user.LastName = claim.Value; 
                    break;
                case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                    user.EmailAddress = claim.Value; 
                    break;
                case "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
                    user.Role = claim.Value; 
                    break;
                default:
                    break;
            }
        }

        return user;
    }

    public static User ToUserInfo(this ClaimsIdentity identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        var claims = identity.Claims;

        var user = new User();
        foreach (var claim in claims)
        {
            switch (claim.Type)
            {
                case "contactId":
                    user.ContactId = claim.Value;
                    break;
                case "uniqueReference":
                    user.UniqueReference = claim.Value;
                    break;
                case "firstName":
                    user.FirstName = claim.Value;
                    break;
                case "lastName":
                    user.LastName = claim.Value;
                    break;
                case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                    user.EmailAddress = claim.Value;
                    break;
                case "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
                    user.Role = claim.Value;
                    break;
                default:
                    break;
            }
        }

        return user;
    }

    public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var loggedInUserId =  principal.FindFirst(WebAppConstants.IdentityKeys.PTSUserId)?.Value;
        if (!string.IsNullOrWhiteSpace(loggedInUserId))
        {
            return new Guid(loggedInUserId);
        }

        throw new InvalidDataException("Invalid userId provided");
    }

    public static Guid GetLoggedInContactId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var loggedInContactId = principal.FindFirst("ContactId")?.Value;
        if (!string.IsNullOrWhiteSpace(loggedInContactId))
        {
            return new Guid(loggedInContactId);
        }

     
        throw new InvalidDataException("Invalid contactId provided");
    }

    public static string GetLoggedInUserName(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.FindFirstValue(ClaimTypes.Name);
    }

    public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.FindFirstValue(ClaimTypes.Email);
    }
}
