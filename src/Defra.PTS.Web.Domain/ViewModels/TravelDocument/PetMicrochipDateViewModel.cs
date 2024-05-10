using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetMicrochipDateViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"When was your pet microchipped or last scanned?";

    public int? Day { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public DateTime? MicrochippedDate
    {
        get
        {
            try
            {
                return new DateTime(Year.GetValueOrDefault(), Month.GetValueOrDefault(), Day.GetValueOrDefault(), 0, 0, 0, 0, DateTimeKind.Utc);
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
