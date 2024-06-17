using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public abstract class TravelDocumentFormPage
{
    public bool IsCompleted { get; set; }

    public string Locale { get; set; } = "cy";

    public abstract void TrimUnwantedData();

    public abstract void ClearData();

    public abstract Enums.TravelDocumentFormPageType PageType { get; }
}
