using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Domain.ViewModels;
[ExcludeFromCodeCoverage]
public class FileUploadItem
{
    public string Id { get; set; }

    public string FileName { get; set; }

    public string FileExtension
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                var extension = Path.GetExtension(FileName) ?? string.Empty;
                return extension.Replace(".", string.Empty);
            }

            return string.Empty;
        }
    }

    public long Length { get; set; }

    public string LengthAsString
    {
        get
        {
            float size = Length;
            float sizeKb = 1024.0f;
            float sizeMb = sizeKb * sizeKb;
            float sizeGb = sizeMb * sizeKb;
            float sizeTerra = sizeGb * sizeKb;

            if (size < sizeMb)
                return GetSizeString(size, sizeKb, "KB");
            else if (size < sizeGb)
                return GetSizeString(size, sizeMb, "MB");
            else if (size < sizeTerra)
                return GetSizeString(size, sizeGb, "GB");

            return GetSizeString(size, sizeTerra, "TB");
        }
    }

    public decimal LengthInMB
    {
        get
        {
            if (Length == 0) return 0;

            return (decimal)Length / 1048576;
        }
    }

    public string ContentType { get; set; }

    public byte[] FileContent { get; set; }

    private static string GetSizeString(float dividend, float divideBy, string suffix)
    {
        var valueText = (dividend / divideBy).ToString("0.00");

        return $"{valueText} {suffix}";
    }
}
