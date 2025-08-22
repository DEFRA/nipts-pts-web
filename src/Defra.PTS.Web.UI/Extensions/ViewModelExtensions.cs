using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.UI.Extensions;

public static class ViewModelExtensions
{
    public static bool DoesPageMeetPreConditions(this TravelDocumentViewModel vm, TravelDocumentFormPageType formPage, out string actionName)
    {
        actionName = string.Empty;

        if (formPage == TravelDocumentFormPageType.PetKeeperUserDetails)
            return true;

        if (!vm.PetKeeperUserDetails.IsCompleted)
            return SetActionAndReturnFalse("PetKeeperUserDetails", out actionName);

        if (formPage == TravelDocumentFormPageType.PetKeeperName)
            return true;

        if (!vm.PetKeeperName.IsCompleted && vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
            return SetActionAndReturnFalse("PetKeeperName", out actionName);

        if (IsAddressPage(formPage))
        {
            if (!vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
                return true;

            if (formPage == TravelDocumentFormPageType.PetKeeperAddress && !vm.PetKeeperPostcode.IsCompleted)
                return SetActionAndReturnFalse("PetKeeperPostcode", out actionName);

            return true;
        }

        if (vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
        {
            var addressCompleted = vm.PetKeeperPostcode.IsCompleted && vm.PetKeeperAddress.IsCompleted;
            var manualAddressCompleted = vm.PetKeeperAddressManual.IsCompleted;
            if (!(addressCompleted || manualAddressCompleted))
                return SetActionAndReturnFalse("PetKeeperPostcode", out actionName);
        }

        if (formPage == TravelDocumentFormPageType.PetKeeperPhone)
            return true;

        if (!vm.PetKeeperPhone.IsCompleted && vm.PetKeeperUserDetails.PetOwnerDetailsRequired)
            return SetActionAndReturnFalse("PetKeeperPhone", out actionName);

        if (formPage == TravelDocumentFormPageType.PetMicrochip ||
            formPage == TravelDocumentFormPageType.PetMicrochipNotAvailable ||
            formPage == TravelDocumentFormPageType.PetMicrochipDate ||
            formPage == TravelDocumentFormPageType.PetSpecies ||
            formPage == TravelDocumentFormPageType.PetName ||
            formPage == TravelDocumentFormPageType.PetGender ||
            formPage == TravelDocumentFormPageType.PetAge ||
            formPage == TravelDocumentFormPageType.PetColour ||
            formPage == TravelDocumentFormPageType.PetFeature ||
            formPage == TravelDocumentFormPageType.Declaration ||
            formPage == TravelDocumentFormPageType.Acknowledgement)
        {
            if (formPage == TravelDocumentFormPageType.PetMicrochip && !vm.PetMicrochip.IsCompleted)
                return SetActionAndReturnFalse("PetMicrochip", out actionName);

            if (formPage == TravelDocumentFormPageType.PetMicrochipDate && !vm.PetMicrochipDate.IsCompleted)
                return SetActionAndReturnFalse("PetMicrochipDate", out actionName);

            if (formPage == TravelDocumentFormPageType.PetSpecies && !vm.PetSpecies.IsCompleted)
                return SetActionAndReturnFalse("PetSpecies", out actionName);

            if (vm.PetSpecies.PetSpecies.HasBreed())
            {
                if (formPage == TravelDocumentFormPageType.PetBreed)
                    return true;

                if (!vm.PetBreed.IsCompleted)
                    return SetActionAndReturnFalse("PetBreed", out actionName);
            }

            if (formPage == TravelDocumentFormPageType.PetName && !vm.PetName.IsCompleted)
                return SetActionAndReturnFalse("PetName", out actionName);

            if (formPage == TravelDocumentFormPageType.PetGender && !vm.PetGender.IsCompleted)
                return SetActionAndReturnFalse("PetGender", out actionName);

            if (formPage == TravelDocumentFormPageType.PetAge && !vm.PetAge.IsCompleted)
                return SetActionAndReturnFalse("PetAge", out actionName);

            if (formPage == TravelDocumentFormPageType.PetColour && !vm.PetColour.IsCompleted)
                return SetActionAndReturnFalse("PetColour", out actionName);

            if (formPage == TravelDocumentFormPageType.PetFeature && !vm.PetFeature.IsCompleted)
                return SetActionAndReturnFalse("PetFeature", out actionName);

            if (formPage == TravelDocumentFormPageType.Declaration && !vm.Declaration.IsCompleted)
                return SetActionAndReturnFalse("Declaration", out actionName);

            return true;
        }

        return true;
    }

    private static bool SetActionAndReturnFalse(string action, out string actionName)
    {
        actionName = action;
        return false;
    }

    private static bool IsAddressPage(TravelDocumentFormPageType formPage)
    {
        return formPage == TravelDocumentFormPageType.PetKeeperPostcode ||
               formPage == TravelDocumentFormPageType.PetKeeperAddress ||
               formPage == TravelDocumentFormPageType.PetKeeperAddressManual;
    }
}
