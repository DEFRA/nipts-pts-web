using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class ApplicationCertificateNotFoundException : Exception
{
    public ApplicationCertificateNotFoundException()
    {
    }

    public ApplicationCertificateNotFoundException(string message)
        : base(message)
    {
    }

    public ApplicationCertificateNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}