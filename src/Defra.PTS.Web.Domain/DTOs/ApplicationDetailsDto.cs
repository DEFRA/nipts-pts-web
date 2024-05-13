using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Application.DTOs.Services;

public class ApplicationDetailsDto
{
    public Guid ApplicationId { get; set; }
    public string ReferenceNumber { get; set; }
    public string ReferenceNumberAsQRCode { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; }

    public MicrochipInformationDto MicrochipInformation { get; set; } = new();
    public PetDetailsDto PetDetails { get; set; } = new();
    public PetKeeperDetailsDto PetKeeperDetails { get; set; } = new();
    public DeclarationDto Declaration { get; set; } = new();

    public bool IsApproved => !string.IsNullOrWhiteSpace(Status) && Status.Equals("Approved");

    public ActionLinksDto ActionLinks { get; set; } = new();
}

public class ApplicationCertificateDto : ApplicationDetailsDto
{
    public CertificateIssuedDto CertificateIssued { get; set; } = new();
    public CertificateIssuingAuthorityDto CertificateIssuingAuthority { get; set; } = new();
}

public class CertificateIssuedDto
{
    public string DocumentReferenceNumber { get; set; }
    public DateTime? DocumentIssueDate { get; set; }
    public string DocumentReferenceNumberAsQRCode { get; set; }
}

public class CertificateIssuingAuthorityDto
{
    public string AuthorityName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string TownOrCity { get; set; }
    public string Postcode { get; set; }

    public string SignedOnBehalfOfAPHA { get; set; }
}

public class MicrochipInformationDto
{
    public string MicrochipNumber { get; set; }
    public DateTime MicrochipDate { get; set; }
    public string MicrochipImplantLocation { get; set; }

}

public class PetDetailsDto
{
    public string Name { get; set; }

    public string Species { get; set; }

    public string Breed { get; set; }

    public bool HasBreed { get; set; }

    public string Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public string Colour { get; set; }

    public string Feature { get; set; }
}

public class PetKeeperDetailsDto
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string TownOrCity { get; set; }
    public string Postcode { get; set; }

    public string Phone { get; set; }
}

public class DeclarationDto
{
    public string PetOwnerName { get; set; }
    public DateTime ApplicationDate { get; set; }
}

public class ActionLinksDto
{
    public Guid Id { get; set; }
    public bool ShowDownloadLink { get; set; }
    public PdfType PdfType { get; set; }
}