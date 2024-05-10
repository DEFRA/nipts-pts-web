namespace Defra.PTS.Web.Domain.Models;

public class User
{
    public string UserId { get; set; }
    public string ContactId { get; set; }

    public string UniqueReference { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string Role { get; set; }

}
