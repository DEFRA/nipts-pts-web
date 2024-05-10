using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.UI.Extensions;

public static class ViewModelExtensions
{
    public static bool DoesPageMeetPreConditions(this TravelDocumentViewModel vm, TravelDocumentFormPageType formPage, out string actionName)
    {
        var addressPages = new List<TravelDocumentFormPageType>
        {
            TravelDocumentFormPageType.PetKeeperPostcode,
            TravelDocumentFormPageType.PetKeeperAddress,
            TravelDocumentFormPageType.PetKeeperAddressManual
        };

        actionName = string.Empty;

        // PetKeeperUserDetails
        if (formPage == TravelDocumentFormPageType.PetKeeperUserDetails)
        {
            return true;
        }

        if (!vm.PetKeeperUserDetails.IsCompleted)
        {
            actionName = "PetKeeperUserDetails";
            return false;
        }

        // PetKeeperName
        if (formPage == TravelDocumentFormPageType.PetKeeperName)
        {
            return true;
        }

        if (!vm.PetKeeperName.IsCompleted && vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
        {
            actionName = "PetKeeperName";
            return false;
        }

        // Address
        if (addressPages.Contains(formPage))
        {
            if (!vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
            {
                return true;
            }

            switch (formPage)
            {
                case TravelDocumentFormPageType.PetKeeperPostcode:
                case TravelDocumentFormPageType.PetKeeperAddressManual:
                    return true;
                case TravelDocumentFormPageType.PetKeeperAddress:
                    {
                        if (!vm.PetKeeperPostcode.IsCompleted)
                        {
                            actionName = "PetKeeperPostcode";
                            return false;
                        }

                        return true;
                    }
            }
        }

        // Select Address or Manual Address
        if (vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
        {
            var addressCompleted = vm.PetKeeperPostcode.IsCompleted && vm.PetKeeperAddress.IsCompleted;
            var manualAddressCompleted = vm.PetKeeperAddressManual.IsCompleted;
            if (!(addressCompleted || manualAddressCompleted))
            {
                actionName = "PetKeeperPostcode";
                return false;
            }
        }

        // PetKeeperPhone
        if (formPage == TravelDocumentFormPageType.PetKeeperPhone)
        {
            return true;
        }

        if (!vm.PetKeeperPhone.IsCompleted && vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
        {
            actionName = "PetKeeperPhone";
            return false;
        }

        // B1: PetMicrochip
        if (formPage == TravelDocumentFormPageType.PetMicrochip)
        {
            return true;
        }

        if (!vm.PetMicrochip.IsCompleted)
        {
            actionName = "PetMicrochip";
            return false;
        }

        // B1.1: PetMicrochipNotAvailable
        if (formPage == TravelDocumentFormPageType.PetMicrochipNotAvailable)
        {
            return true;
        }

        // B2: PetMicrochipDate
        if (formPage == TravelDocumentFormPageType.PetMicrochipDate)
        {
            return true;
        }

        if (!vm.PetMicrochipDate.IsCompleted)
        {
            actionName = "PetMicrochipDate";
            return false;
        }

        // B3: PetSpecies
        if (formPage == TravelDocumentFormPageType.PetSpecies)
        {
            return true;
        }

        if (!vm.PetSpecies.IsCompleted)
        {
            actionName = "PetSpecies";
            return false;
        }

        // B3.1: PetBreed
        if (vm.PetSpecies.PetSpecies.HasBreed())
        {
            if (formPage == TravelDocumentFormPageType.PetBreed)
            {
                return true;
            }

            if (!vm.PetBreed.IsCompleted)
            {
                actionName = "PetBreed";
                return false;
            }
        }

        // B4: PetName
        if (formPage == TravelDocumentFormPageType.PetName)
        {
            return true;
        }

        if (!vm.PetName.IsCompleted)
        {
            actionName = "PetName";
            return false;
        }

        // B5: PetGender
        if (formPage == TravelDocumentFormPageType.PetGender)
        {
            return true;
        }

        if (!vm.PetGender.IsCompleted)
        {
            actionName = "PetGender";
            return false;
        }

        // B6: PetAge
        if (formPage == TravelDocumentFormPageType.PetAge)
        {
            return true;
        }

        if (!vm.PetAge.IsCompleted)
        {
            actionName = "PetAge";
            return false;
        }

        // B7: PetColour
        if (formPage == TravelDocumentFormPageType.PetColour)
        {
            return true;
        }

        if (!vm.PetColour.IsCompleted)
        {
            actionName = "PetColour";
            return false;
        }

        // B8: PetFeature
        if (formPage == TravelDocumentFormPageType.PetFeature)
        {
            return true;
        }

        if (!vm.PetFeature.IsCompleted)
        {
            actionName = "PetFeature";
            return false;
        }

        // B9: Declaration
        if (formPage == TravelDocumentFormPageType.Declaration)
        {
            return true;
        }

        if (!vm.Declaration.IsCompleted)
        {
            actionName = "Declaration";
            return false;
        }

        // B10: Acknowledgement
        if (formPage == TravelDocumentFormPageType.Acknowledgement)
        {
            return true;
        }

        return true;
    }
}
