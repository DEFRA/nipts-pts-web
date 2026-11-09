using Defra.PTS.Web.Application.DTOs.Features;
using MediatR;

namespace Defra.PTS.Web.Application.Features.Users.Queries;

public class GetIsUserSuspendedQueryRequest(Guid contactId) : IRequest<GetIsUserSuspendedQueryResponse>
{
    public Guid ContactId { get; } = contactId;
}
