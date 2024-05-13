using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class ApplicationDetailsNotFoundException : Exception
{
    public ApplicationDetailsNotFoundException()
    {
    }

    public ApplicationDetailsNotFoundException(string message)
        : base(message)
    {
    }

    public ApplicationDetailsNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}