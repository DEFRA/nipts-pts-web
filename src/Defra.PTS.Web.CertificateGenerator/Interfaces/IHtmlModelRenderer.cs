namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface IHtmlModelRenderer<TModel> : IModelRenderer<TModel, string>
{
}

public interface IHtmlModelRenderer : IModelRenderer<string>
{
}