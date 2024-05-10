using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Application.Services.Interfaces;

public interface IUserService
{
    Task<Guid> AddUserAsync(User user);
    Task<Guid> AddOwnerAsync(User user, TravelDocumentViewModel travelDocument);
    Task<Guid> UpdateUserAsync(string emailAddress); 
    Task<Guid> UpdateUserAddressAsync(string emailAddress, Guid? addressId);
    Task<UserDetailDto> GetUserDetail(Guid userId);
    Task<Guid> AddAddressAsync(AddressType addressType, TravelDocumentViewModel travelDocument);
}
