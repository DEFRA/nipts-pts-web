using AutoMapper;
using Defra.PTS.Web.Application.Helpers;

namespace Defra.PTS.Web.Application.Mapping.Converters;

public class ConvertDisplayStatus : IValueConverter<string, string>
{
    public string Convert(string status, ResolutionContext context)
    {
        return ApplicationHelper.MapStatusToDisplayStatus(status);
    }
}
