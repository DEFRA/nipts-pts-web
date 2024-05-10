using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetUserDetailQueryResponse
{
    public Guid UserId { get; set; }
    public UserDetailDto UserDetail { get; set; } = new UserDetailDto();
}
