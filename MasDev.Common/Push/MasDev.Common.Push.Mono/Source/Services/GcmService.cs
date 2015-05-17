
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;
using PushSharp.Core;

namespace MasDev.Common.Push.Services
{
    public class GcmService : IGcmService
    {
        public Task SendAsync(string deviceId, object data)
        {
            if(string.IsNullOrWhiteSpace(AuthorizationToken))
                throw new ArgumentException("Specify AuthorizationToken first");
            var task = new GcmSendTask(AuthorizationToken, deviceId, data);
            return task.Completion.Task;
        }

        public string AuthorizationToken { get; set; }
    }


    class GcmSendTask
    {
        public TaskCompletionSource<INotification> Completion { get; private set; }

        public GcmSendTask(string gcmKey, string deviceId, object data)
        {
            Completion = new TaskCompletionSource<INotification>();
            var broker = new PushBroker();

            broker.OnNotificationSent += NotificationSent;
            broker.OnChannelException += ChannelException;
            broker.OnServiceException += ServiceException;
            broker.OnNotificationFailed += NotificationFailed;
            broker.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            broker.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            broker.OnChannelCreated += ChannelCreated;
            broker.OnChannelDestroyed += ChannelDestroyed;

            broker.RegisterGcmService(new GcmPushChannelSettings(gcmKey));
            broker.QueueNotification(new GcmNotification().ForDeviceRegistrationId(deviceId).WithJson(JsonConvert.SerializeObject(data)));

            broker.StopAllServices();
        }

        private void ChannelDestroyed(object sender)
        {
            // TODO handle?
        }

        private void ChannelCreated(object sender, IPushChannel pushchannel)
        {
            // TODO handle?
        }

        private void DeviceSubscriptionChanged(object sender, string oldsubscriptionid, string newsubscriptionid, INotification notification)
        {
            Completion.SetException(new DeviceIdChangedException(oldsubscriptionid, newsubscriptionid));
        }

        private void DeviceSubscriptionExpired(object sender, string expiredsubscriptionid, DateTime expirationdateutc, INotification notification)
        {
            Completion.SetException(new DeviceSubscriptionExpiredException(expiredsubscriptionid, expirationdateutc));
        }

        private void NotificationFailed(object sender, INotification notification, Exception error)
        {
            Completion.SetException(new PushFailedException(error));
        }

        private void ServiceException(object sender, Exception error)
        {
            Completion.SetException(error);
        }

        private void ChannelException(object sender, IPushChannel pushchannel, Exception error)
        {
            Completion.SetException(error);
        }

        private void NotificationSent(object sender, INotification notification)
        {
            Completion.SetResult(notification);
        }
    }
}
