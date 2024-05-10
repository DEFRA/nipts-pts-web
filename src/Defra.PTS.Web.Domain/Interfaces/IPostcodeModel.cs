using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.Interfaces;

public interface IPostcodeModel
{
    string Postcode { get; set; }

    PostcodeRegion PostcodeRegion { get; set; }
}
