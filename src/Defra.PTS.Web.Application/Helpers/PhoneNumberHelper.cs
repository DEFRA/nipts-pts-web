namespace Defra.PTS.Web.Application.Helpers;

using PhoneNumbers;

public static class PhoneNumberHelper
{
    private readonly static PhoneNumberUtil _phoneNumberUtil = PhoneNumberUtil.GetInstance();

    public static bool PhoneNumberHasInvalidCharacters(string phone)
    {
        phone = phone.Trim();

        var invalid = phone.Contains("--") || 
            phone.Contains("((") || 
            phone.Contains("))") || 
            phone.Contains('*') ||
            phone.Contains('/') ||
            phone.Contains("++");

        return invalid;
    }

    public static bool IsValidUKPhoneNumber(string phone)
    {
        return IsValidPhoneNumber(phone);
    }

    public static bool IsValidPhoneNumber(string phone)
    {
        var validCountryCodes = new List<string>() { "GB", "GG" };

        phone = phone.Trim();

        if (PhoneNumberHasInvalidCharacters(phone))
        {
            return false;
        }

        bool isValidNumber = false;
        foreach (var countryCode in validCountryCodes)
        {
            var phoneNumber = _phoneNumberUtil.Parse(phone, countryCode);

            isValidNumber = _phoneNumberUtil.IsValidNumberForRegion(phoneNumber, countryCode);

            if (isValidNumber)
            {
                break;
            }
        }
        
        return isValidNumber;
    }
}