using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using MediatR;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Commands;

public class CreateTravelDocumentRequest : IRequest<CreateTravelDocumentResponse>
{
    public TravelDocumentViewModel Model { get; }
    public User User { get; }
    public CreateTravelDocumentRequest(TravelDocumentViewModel viewModel, User user)
    {
        Model = viewModel;
        User = user;
    }
}
