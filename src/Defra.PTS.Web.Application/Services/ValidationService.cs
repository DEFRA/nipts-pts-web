using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Services;

[ExcludeFromCodeCoverage]
public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;
    public ValidationService(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        _serviceProvider = serviceProvider;
    }

    public ValidationResult ValidateTravelDocument(TravelDocumentViewModel model)
    {
        var validator = _serviceProvider.GetRequiredService<IValidator<TravelDocumentViewModel>>();
        return validator.Validate(model);
    }


    public ValidationResult Validate<T>(T model) where T : TravelDocumentFormPage
    {
        model.TrimUnwantedData();

        var validator = _serviceProvider.GetRequiredService<IValidator<T>>();
        return validator.Validate(model);
    }
}