namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

#pragma warning disable CS8632
public class PetMicrochipDateViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"When was your pet microchipped or last scanned?";

    public string? Day { get; set; }

    public string? Month { get; set; }

    public string? Year { get; set; }

    public DateTime? MicrochippedDate
    {
        get
        {
            try
            {
                _ = int.TryParse(Day, out int day);
                _ = int.TryParse(Month, out int month);
                _ = int.TryParse(Year, out int year);
                return new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc);
            }
            catch
            {
                return null;
            }
        }
    }

    public DateTime? BirthDate { get; set; }


    public override Enums.TravelDocumentFormPageType PageType => Enums.TravelDocumentFormPageType.PetMicrochipDate;

    public override void TrimUnwantedData()
    {
    }

    public override void ClearData()
    {
        IsCompleted = default;
        Day = default;
        Month = default;
        Year = default;
    }
}
