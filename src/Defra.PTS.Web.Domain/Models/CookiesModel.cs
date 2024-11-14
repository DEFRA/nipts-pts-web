namespace Defra.PTS.Web.Domain.Models;

public class CookiesModel
{
    public string GaCookieAcceptYesNo { get; set; } = string.Empty;
    public string MeasurementId { get; set; } = string.Empty;
    public bool SeenCookieMessage { get; set; } = false;
    public bool Saved { get; set; } = false;
}
