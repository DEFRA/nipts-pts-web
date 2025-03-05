using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

public interface ICustomBrowser
{
    Task<IPage> NewPageAsync();
}
