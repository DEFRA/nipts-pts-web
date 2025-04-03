using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.Components;
using Defra.PTS.Web.UI.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.UI.ViewComponents;

[ExcludeFromCodeCoverage]
public class PetKeeperDetailsCard : ViewComponent
{
    public IViewComponentResult Invoke(string viewName = null)
    {
        var dto = TempData.GetTravelDocument();

        var address = dto.GetPetOwnerAddress();

        var model = new PetKeeperDetailsCvm
        {
            Name = dto.GetPetOwnerName(),
            NameUrl = WebAppConstants.Pages.TravelDocument.PetKeeperName,

            Email = dto.PetKeeperUserDetails.Email,

            Phone = dto.GetPetOwnerPhone(),
            PhoneUrl = WebAppConstants.Pages.TravelDocument.PetKeeperPhone,

            Address = new Address
            {
                AddressLineOne = address.AddressLineOne,
                AddressLineTwo = address.AddressLineTwo,
                TownOrCity = address.TownOrCity,
                Postcode = address.PostCode,
                County = address.County,
            },

            AddressUrl = dto.PetKeeperAddressManual.IsCompleted ? WebAppConstants.Pages.TravelDocument.PetKeeperAddressManual : WebAppConstants.Pages.TravelDocument.PetKeeperPostcode
        };

        return View(viewName ?? "Default", model);
    }
}