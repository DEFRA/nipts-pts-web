using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class CreateTravelDocumentResponse
{
    public bool IsSuccess { get; set; }
    public string Reference { get; set; }
    public Guid UserId { get; set; }
    public ApplicationDto Application { get; set; } = new();
}
