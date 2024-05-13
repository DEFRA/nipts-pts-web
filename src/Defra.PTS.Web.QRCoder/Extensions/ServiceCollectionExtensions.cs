using Defra.PTS.Web.QRCoder.Services;
using Defra.PTS.Web.QRCoder.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.QRCoder.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQRCoderServices(this IServiceCollection services)
    {
        services.AddScoped<IQRCodeService, QRCodeService>();

        return services;
    }
}
