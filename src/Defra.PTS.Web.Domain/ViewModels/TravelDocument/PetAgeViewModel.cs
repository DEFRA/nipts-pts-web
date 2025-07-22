using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetAgeViewModel : TravelDocumentFormPage
{
    public static string FormTitle => $"What is your pet's date of birth?";
    
    public string Day { get; set; }

    public string Month { get; set; }

    public string Year { get; set; }
    public DateTime? BirthDate
    {
        get
        {
            try
            {
                if (Year != null && Month != null && Day != null)
                {
                    _ = int.TryParse(Day, out int day);
                    _ = int.TryParse(Month, out int month);
                    _ = int.TryParse(Year, out int year);
                    return new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }

    public DateTime MicrochippedDate { get; set; }

    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetAge;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Day = default;
        Month = default;
        Year = default;
        MicrochippedDate = default;
    }
}
