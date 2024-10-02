using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Linq;
using static Defra.PTS.Web.UI.Constants.WebAppConstants;

namespace Defra.PTS.Web.Application.Extensions;

public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonConvert.SerializeObject(value);
    }

    public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out object o);
        return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
    }

    public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        var o = tempData.Peek(key);
        return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
    }

    public static void SetTravelDocument(this ITempDataDictionary tempData, TravelDocumentViewModel model)
    {
        tempData.Set(TempDataKey.TravelDocumentViewModel, model);
    }

    public static void SetHasUserUsedMagicWord(this ITempDataDictionary tempData, MagicWordViewModel model)
    {
        tempData.Set(TempDataKey.HasUserUsedMagicWord, model);
    }

    public static TravelDocumentViewModel GetTravelDocument(this ITempDataDictionary tempData, bool createIfNull = true)
    {
        var result = tempData.Peek<TravelDocumentViewModel>(TempDataKey.TravelDocumentViewModel);
        if (result == null && createIfNull)
        {
            result = new TravelDocumentViewModel();
            tempData.Set(TempDataKey.TravelDocumentViewModel, result);
        }

        return result;
    }

    public static MagicWordViewModel GetHasUserUsedMagicWord(this ITempDataDictionary tempData, bool createIfNull = true)
    {
        var result = tempData.Peek<MagicWordViewModel>(TempDataKey.HasUserUsedMagicWord);
        if (result == null && createIfNull)
        {
            result = new MagicWordViewModel();
        }

        return result;
    }

    public static void RemoveTravelDocument(this ITempDataDictionary tempData)
    {
        tempData.Remove(TempDataKey.TravelDocumentViewModel);
    }

    public static void RemoveHasUserUsedMagicWord(this ITempDataDictionary tempData)
    {
        tempData.Remove(TempDataKey.HasUserUsedMagicWord);
    }

    public static List<Guid> GetFormSubmissionQueue(this ITempDataDictionary tempData)
    {
        var result = tempData.Peek<List<Guid>>(TempDataKey.FormSubmissionQueue);
        if (result == null)
        {
            result = [];
            tempData.Set(TempDataKey.FormSubmissionQueue, result);
        }

        return result;
    }

    public static void AddToFormSubmissionQueue(this ITempDataDictionary tempData, Guid id)
    {
        var model = tempData.GetFormSubmissionQueue();
        if (!model.Contains(id))
        {
            model.Add(id);
            tempData.Set(TempDataKey.FormSubmissionQueue, model);
        }
    }

    public static void RemoveFromFormSubmissionQueue(this ITempDataDictionary tempData, Guid id)
    {
        var model = tempData.GetFormSubmissionQueue();
        if (!model.Contains(id))
        {
            return;
        }
        model.Remove(id);
        tempData.Set(TempDataKey.FormSubmissionQueue, model);
    }

    public static bool IsInFormSubmissionQueue(this ITempDataDictionary tempData, Guid id)
    {
        var model = tempData.GetFormSubmissionQueue();
        return model.Contains(id);
    }

    public static void SetApplicationReference(this ITempDataDictionary tempData, string applicationReference)
    {
        if (!tempData.ContainsKey(TempDataKey.ApplicationReference))
        {
            tempData.Add(TempDataKey.ApplicationReference, applicationReference);
        }        
    }
    public static string GetApplicationReference(this ITempDataDictionary tempData)
    {
        var o = tempData.Peek(TempDataKey.ApplicationReference);
        return o == null ? null : (string)o;
    }

}