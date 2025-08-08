using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Application.DTOs.Services;

public class ApplicationSummaryDto
{
    public Guid ApplicationId { get; set; }

    public string ReferenceNumber { get; set; }

    public string PetName { get; set; }

    public string Species
    {
        get
        {
            return PetSpeciesId.GetDescription();
        }
    }

    public PetSpecies PetSpeciesId { get; set; }

    public string OwnerName { get; set; }

    public string Status { get; set; }

    public DateTime? DateOfApplication { get; set; }

    public DateTime? DocumentIssueDate { get; set; }
}
