using System.IO;

namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface IPdfModelRenderer<TModel> : IModelRenderer<TModel, Stream>
{
}

public interface IPdfModelRenderer : IModelRenderer<Stream>
{
}