
using System;

namespace MasDev.Common.Push
{
    public class DeviceSubscriptionExpiredException : PushException
    {
        public string ExpiredSubscriptionId { get; private set; }
        public DateTime ExpirationDateUtc { get; private set; }

        public DeviceSubscriptionExpiredException(string expiredsubscriptionid, DateTime expirationdateutc)
        {
            ExpirationDateUtc = expirationdateutc;
            ExpiredSubscriptionId = expiredsubscriptionid;
        }
    }
}
