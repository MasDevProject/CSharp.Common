using System.Threading.Tasks;

namespace MasDev.Common.Push
{
    public interface IGcmService
    {
        string AuthorizationToken { get; set; }

        Task SendAsync(string deviceId, object data);
    }
}
