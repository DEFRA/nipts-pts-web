using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.Results;

namespace Defra.PTS.Web.Application.Services.Interfaces;

public interface IValidationService
{
    ValidationResult ValidateTravelDocument(TravelDocumentViewModel model);

    ValidationResult Validate<T>(T model) where T : TravelDocumentFormPage;
}
