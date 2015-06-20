using MasDev.Common.Push.Services;
using MasDev.Common.Push.Wrappers;

namespace MasDev.Common.Push
{
    public class PushServices : IPushServices
    {
        readonly IGcmService _gcm;
		readonly IApplePushService _applePushService;

		public PushServices ()
		{
			_gcm = new GcmService ();
			_applePushService = new ApplePushService();
		}

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
