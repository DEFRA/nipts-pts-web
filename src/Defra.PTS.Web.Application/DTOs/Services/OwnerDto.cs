namespace Defra.PTS.Web.Application.DTOs.Services;

public class OwnerDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public int OwnerTypeId { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public AddressDto Address { get; set; }
}
