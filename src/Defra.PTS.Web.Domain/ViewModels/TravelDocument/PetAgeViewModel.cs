using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetAgeViewModel : TravelDocumentFormPage
{
    public string FormTitle => $"What is your pet's date of birth?";

    public DateTime? BirthDate
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

    public int? Day { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

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
