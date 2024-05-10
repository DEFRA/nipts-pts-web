namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class TravelDocumentViewModel
{
    public bool IsApplicationInProgress { get; set; }
    public Guid RequestId { get; set; } = Guid.NewGuid();
    public bool IsSubmitted { get; set; }

    // PetKeeperUserDetails
    public PetKeeperUserDetailsViewModel PetKeeperUserDetails { get; set; } = new PetKeeperUserDetailsViewModel();

    // PetKeeperName 
    public PetKeeperNameViewModel PetKeeperName { get; set; } = new PetKeeperNameViewModel();

    // PetKeeperPostcode 
    public PetKeeperPostcodeViewModel PetKeeperPostcode { get; set; } = new PetKeeperPostcodeViewModel();

    // PetKeeperAddress 
    public PetKeeperAddressViewModel PetKeeperAddress { get; set; } = new PetKeeperAddressViewModel();

    // PetKeeperAddressManual 
    public PetKeeperAddressManualViewModel PetKeeperAddressManual { get; set; } = new PetKeeperAddressManualViewModel();

    // PetKeeperPhone 
    public PetKeeperPhoneViewModel PetKeeperPhone { get; set; } = new PetKeeperPhoneViewModel();

    // PetMicrochip
    public PetMicrochipViewModel PetMicrochip { get; set; } = new PetMicrochipViewModel();

    // PetMicrochipNotAvailable
    public PetMicrochipNotAvailableViewModel PetMicrochipNotAvailable { get; set; } = new PetMicrochipNotAvailableViewModel();

    // PetMicrochipDate
    public PetMicrochipDateViewModel PetMicrochipDate { get; set; } = new PetMicrochipDateViewModel();

    // PetSpecies
    public PetSpeciesViewModel PetSpecies { get; set; } = new PetSpeciesViewModel();

    // PetBreed
    public PetBreedViewModel PetBreed { get; set; } = new PetBreedViewModel();

    // PetName
    public PetNameViewModel PetName { get; set; } = new PetNameViewModel();

    // PetGender
    public PetGenderViewModel PetGender { get; set; } = new PetGenderViewModel();

    // PetAge
    public PetAgeViewModel PetAge { get; set; } = new PetAgeViewModel();

    // PetColour
    public PetColourViewModel PetColour { get; set; } = new PetColourViewModel();

    // PetFeature
    public PetFeatureViewModel PetFeature { get; set; } = new PetFeatureViewModel();

    // Declaration
    public DeclarationViewModel Declaration { get; set; } = new DeclarationViewModel();

    // Acknowledgement
    public AcknowledgementViewModel Acknowledgement { get; set; } = new AcknowledgementViewModel();
}
