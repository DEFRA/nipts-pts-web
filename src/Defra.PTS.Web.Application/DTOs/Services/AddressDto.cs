using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.DTOs.Services;

public class AddressDto
{
    [ExcludeFromCodeCoverage]
    public Guid Id { get; set; }
    public string AddressLineOne { get; set; }
    public string AddressLineTwo { get; set; }
    public string TownOrCity { get; set; }
    public string County { get; set; }
    public string PostCode { get; set; }
    public string CountryName { get; set; }
    public string AddressType { get; set; }
    public bool? IsActive { get; set; }
    [ExcludeFromCodeCoverage]
    public Guid? CreatedBy { get; set; }
    [ExcludeFromCodeCoverage]
    public DateTime? CreatedOn { get; set; }
    [ExcludeFromCodeCoverage]
    public Guid? UpdatedBy { get; set; }
    [ExcludeFromCodeCoverage]
    public DateTime? UpdatedOn { get; set; }
}
