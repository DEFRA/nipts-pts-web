using Defra.PTS.Web.CertificateGenerator.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface IModelRenderer<TModel, TResult>
{
    Task<RenderResult<TResult>> RenderAsync(TModel model, CancellationToken cancellationToken);
}

public interface IModelRenderer<TResult>
{
    Task<RenderResult<TResult>> RenderAsync<TModel>(TModel model, CancellationToken cancellationToken);

    Task<RenderResult<TResult>> RenderAsync(object model, CancellationToken cancellationToken);
}