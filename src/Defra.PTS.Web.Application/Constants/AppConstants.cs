namespace Defra.PTS.Web.Application.Constants;

public static class AppConstants
{
    public static class RegularExpressions
    {
        public const string DigitOnly = @"^[0-9]+$";

        public const string AlphaNumeric = @"^[0-9a-zA-Z ]+$";

        // Defra Regular Expression for email address
        public const string EmailAddress = @"^\w+([-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        // Defra Regular Expression for Address text
        public const string AddressText = @"^[a-zA-Z0-9\s-./()]*$";

        // Defra Regular Expression for Postcode
        public const string UKPostcode = @"^[a-zA-Z0-9\s]*$";

        // tweaked to be alot more accepting as was too locked down
        public const string UKPhone = @"^[0-9\s+]*$";

        // Defra Regular Expression for Name (using only letters, hyphens or apostrophes)
        public const string Name = @"^[a-zA-Z\s-']*$";

        // Defra Regular Expression for Name (using only letters, numbers, hyphens or apostrophes)
        public const string NameWithNumbers = @"^[a-zA-Z0-9\s-']*$";

        // Defra Regular Expression: Enter a text using only letters, numbers, brackets, full stops, commas, hyphens, underscores, forward slashes or ampersands
        public const string TextMisc = @"^[a-zA-Z0-9\s-_.,/()&]*$";
    }

    public static class ApplicationStatus
    {
        public const string UNSUCCESSFUL = "Unsuccessful";
        public const string REVOKED = "Cancelled";
        public const string APPROVED = "Approved";
        public const string AWAITINGVERIFICATION = "Awaiting Verification";
    }

    public static class Values
    {
        public const int QRCodePixelsPerModule = 4;

        public const int PetMaxAgeInYears = 34;
        public const string OtherColourName = "Other";
    }

    public static class ContentTypes
    {
        public const string Pdf = "application/pdf";
    }

    public static class MaxLength
    {
        // PetColour
        public const int PetColourOther = 150;

        // PetFeature
        public const int PetFeatureDescription = 300;

        // Address
        public const int AddressLine = 250;
        public const int TownOrCity = 250;
        public const int County = 100;
        public const int Postcode = 20;

        // PetKeeperCharity
        public const int PetKeeperCharity = 100;

        // PetKeeperName
        public const int PetKeeperName = 300;

        // PetKeeperPhone
        public const int PetKeeperPhone = 20;

        // PetMicrochipNumber
        public const int PetMicrochipNumber = 15;

        // PetName
        public const int PetName = 300;

    }
}
