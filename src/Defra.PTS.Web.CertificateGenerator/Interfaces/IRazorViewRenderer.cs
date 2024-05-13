using Defra.PTS.Web.CertificateGenerator.Models;

namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface IRazorViewRenderer<TModel> : IModelRenderer<RazorViewModel<TModel>, string>
{
}