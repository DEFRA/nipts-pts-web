using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Domain.Enums;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetColoursQueryRequest : IRequest<GetColoursQueryResponse>
{
    public PetSpecies PetType { get; }
    public GetColoursQueryRequest(PetSpecies petType)
    {
        PetType = petType;
    }
}
