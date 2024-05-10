using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.DTOs.Services;

public class UserDto
{
    [ExcludeFromCodeCoverage]
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public string Telephone { get; set; }
    public Guid? ContactId { get; set; }
    public string Uniquereference { get; set; }
    public DateTime? SignInDateTime { get; set; }
    [ExcludeFromCodeCoverage]
    public DateTime? SignOutDateTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public AddressDto Address { get; set; }
}
