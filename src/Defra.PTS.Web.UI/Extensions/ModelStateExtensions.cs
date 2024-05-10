using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Defra.PTS.Web.Application.Extensions;

public static class ModelStateExtensions
{
    public static bool HasError(this ModelStateDictionary modelState, string key)
    {
        return modelState[key]?.Errors != null && modelState[key].Errors.Any();
    }

    public static string GetErrors(this ModelStateDictionary modelState, string key)
    {
        if (!modelState.HasError(key))
        {
            return string.Empty;
        }

        var errors = modelState
            .Where(v => v.Value.Errors.Any())
            .SelectMany(v => v.Value.Errors.Select(x => new
            {
                v.Key,
                x.ErrorMessage
            }))
            .Where(x => x.Key == key)
            .Select(x => x.ErrorMessage)
            .ToList();

        var message = string.Join(',', errors);
        return message;
    }
}
