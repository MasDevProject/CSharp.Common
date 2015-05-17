

using MasDev.Common.Push.Services;
using MasDev.Common.Push.Wrappers;

namespace MasDev.Common.Push
{
    public class PushServices : IPushServices
    {
        static readonly IGcmService _gcm = new GcmService();
        static readonly IApplePushService _applePushService = new ApplePushService();
        public IGcmService GcmService
        {
            get { return _gcm; }
        }

        public IApplePushService ApplePushService
        {
            get { return _applePushService; }
        }
    }
}
