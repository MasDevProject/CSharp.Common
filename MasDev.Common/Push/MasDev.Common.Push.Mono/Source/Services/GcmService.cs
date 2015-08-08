
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

        void ChannelDestroyed(object sender)
        {
            // TODO handle?
        }

        void ChannelCreated(object sender, IPushChannel pushchannel)
        {
            // TODO handle?
        }

        void DeviceSubscriptionChanged(object sender, string oldsubscriptionid, string newsubscriptionid, INotification notification)
        {
            Completion.SetException(new DeviceIdChangedException(oldsubscriptionid, newsubscriptionid));
        }

        void DeviceSubscriptionExpired(object sender, string expiredsubscriptionid, DateTime expirationdateutc, INotification notification)
        {
            Completion.SetException(new DeviceSubscriptionExpiredException(expiredsubscriptionid, expirationdateutc));
        }

        void NotificationFailed(object sender, INotification notification, Exception error)
        {
            Completion.SetException(new PushFailedException(error));
        }

        void ServiceException(object sender, Exception error)
        {
            Completion.SetException(error);
        }

        void ChannelException(object sender, IPushChannel pushchannel, Exception error)
        {
            Completion.SetException(error);
        }

        void NotificationSent(object sender, INotification notification)
        {
            Completion.SetResult(notification);
        }
    }
}
