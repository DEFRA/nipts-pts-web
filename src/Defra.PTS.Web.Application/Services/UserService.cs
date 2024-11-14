using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Defra.PTS.Web.Application.Extensions; 

namespace Defra.PTS.Web.Application.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserService> _logger;
    public UserService(ILogger<UserService> logger, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Guid> AddUserAsync(User user)
    {
        string apiUrl = _httpClient.BaseAddress + "createuser";
        var postData = new UserDto
        {
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.EmailAddress,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Telephone = string.Empty,
            ContactId = Guid.Parse(user.ContactId),
            Uniquereference = user.UniqueReference,
            SignInDateTime = DateTime.UtcNow,
            CreatedBy = Guid.Parse(user.ContactId),
            CreatedOn = DateTime.UtcNow,
            UpdatedBy = Guid.Parse(user.ContactId),
            UpdatedOn = DateTime.UtcNow,
            Address = new AddressDto()
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to create user, Status code: {response.StatusCode}");
        }

        _logger.LogInformation("User added");

        var userId = await response.Content.ReadFromJsonAsync<Guid>();

        return userId;
    }

    public async Task<Guid> AddOwnerAsync(User user, TravelDocumentViewModel travelDocument)
    {
        string apiUrl = _httpClient.BaseAddress + "createowner";

        var postData = new OwnerDto
        {
            FullName = travelDocument.GetPetOwnerName(),
            Email = user.EmailAddress,
            CreatedBy = Guid.Parse(user.ContactId),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = Guid.Parse(user.ContactId),
            Telephone = travelDocument.GetPetOwnerPhone(),
            OwnerTypeId = 1, // RegisteredOwner
            Address = travelDocument.GetPetOwnerAddress()
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to create owner, Status code: {response.StatusCode}");
        }

        _logger.LogInformation($"User added");

        var ownerId = await response.Content.ReadFromJsonAsync<Guid>();

        return ownerId;
    }

    public async Task<Guid> AddAddressAsync(AddressType addressType, TravelDocumentViewModel travelDocument)
    {
        var postData = travelDocument.GetPetOwnerAddress();
        postData.AddressType = addressType.ToString();

        string apiUrl = _httpClient.BaseAddress + "createaddress";
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to address, Status code: {response.StatusCode}");
        }

        _logger.LogInformation("Address added");

        var addressId = await response.Content.ReadFromJsonAsync<Guid>();

        return addressId;
    }

    public async Task<Guid> UpdateUserAsync(string emailAddress)
    {
        string apiUrl = _httpClient.BaseAddress + "updateuser";
        var postData = new
        {
            email = emailAddress,
            type = "signout"
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to update user, Status code: {response.StatusCode}");
        }

        _logger.LogInformation("User update");

        var userId = await response.Content.ReadFromJsonAsync<Guid>();

        return userId;
    }

    public async Task<Guid> UpdateUserAddressAsync(string emailAddress, Guid? addressId)
    {
        string apiUrl = _httpClient.BaseAddress + "updateuseraddress";
        var postData = new
        {
            email = emailAddress,
            addressId
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Unable to update user, Status code: {response.StatusCode}");
        }

        _logger.LogInformation("User update");

        var userId = await response.Content.ReadFromJsonAsync<Guid>();

        return userId;
    }

    public async Task<UserDetailDto> GetUserDetail(Guid userId)
    {
        string apiUrl = _httpClient.BaseAddress + $"GetUserDetail/{userId}";
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var userDetail = JsonConvert.DeserializeObject<UserDetailDto>(content);
        return userDetail;
    }
}
