using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

public interface IBrowser
{
    Task<IPage> NewPageAsync();
}
