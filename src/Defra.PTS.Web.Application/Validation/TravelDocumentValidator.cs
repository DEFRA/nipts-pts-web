using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Validation;

[ExcludeFromCodeCoverage]
public class TravelDocumentValidator : AbstractValidator<TravelDocumentViewModel>
{
    public TravelDocumentValidator()
    {
    }
}