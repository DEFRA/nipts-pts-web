using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Domain.Enums;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetBreedsQueryRequest : IRequest<GetBreedsQueryResponse>
{
    public PetSpecies PetType { get; }
    public GetBreedsQueryRequest(PetSpecies petType)
    {
        PetType = petType;
    }
}
