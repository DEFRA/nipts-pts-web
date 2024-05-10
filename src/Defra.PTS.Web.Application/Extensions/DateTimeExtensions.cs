namespace Defra.PTS.Web.Application.Extensions;

public static class DateTimeExtensions
{
    public static string ToUtcString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    public static string ToUKDateString(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy");
    }

    public static string ToUKDateString(this DateTime? dateTime)
    {
        if (!dateTime.HasValue) 
        {
            return string.Empty;
        }
        
        return dateTime.Value.ToUKDateString();
    }
}
