using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Models;

namespace Defra.PTS.Web.Application.Services.Interfaces;

public interface IDynamicService
{
    Task AddAddressAsync(User user);

    Task AddApplicationToQueueAsync(ApplicationSubmittedMessage application);
}
