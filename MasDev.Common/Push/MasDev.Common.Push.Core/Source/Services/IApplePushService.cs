using System.Threading.Tasks;

namespace MasDev.Common.Push
{
    public interface IApplePushService
    {
        byte[] Certificate { get; set; }
        string Password { get; set; }

        Task SendAsync(ApplePushNotification notification);

    }

    public class ApplePushNotification
    {
        public string DeviceToken { get; private set; }
        public object Payload { get; private set; }

        public ApplePushNotification(string deviceToken)
        {
            DeviceToken = deviceToken;
        }

        public string Alert { get; set; }

        public int? Badge { get; set; }

        public string Sound { get; set; }
    }
}
