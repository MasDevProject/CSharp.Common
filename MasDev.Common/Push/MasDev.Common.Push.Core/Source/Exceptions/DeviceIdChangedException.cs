
namespace MasDev.Common.Push
{
    public class DeviceIdChangedException : PushException
    {
        public string NewDeviceId { get; private set; }
        public string OldDeviceId { get; private set; }

        public DeviceIdChangedException(string oldDeviceId, string newDeviceId) 
        {
            NewDeviceId = newDeviceId;
            OldDeviceId = oldDeviceId;
        }
    }
}
