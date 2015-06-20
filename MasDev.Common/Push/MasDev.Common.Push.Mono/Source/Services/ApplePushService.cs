
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;

namespace MasDev.Common.Push.Wrappers
{
    public class ApplePushService : IApplePushService
    {
        public Task SendAsync(ApplePushNotification data)
        {
            if (Certificate == null)
                throw new ArgumentException("Specify Certificate first");
            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Specify Password first");

			var task = new ApplePushSendTask(Certificate, Password, data, UseProductionMode);
            return task.Completion.Task;
        }

        public byte[] Certificate { get; set; }

        public string Password { get; set; }

		public bool UseProductionMode { get; set; }
    }


    class ApplePushSendTask
    {
        public TaskCompletionSource<INotification> Completion { get; private set; }

		public ApplePushSendTask(byte[] certificate, string password, ApplePushNotification data, bool productionMode)
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

			broker.RegisterAppleService(new ApplePushChannelSettings(productionMode, certificate, password));
            var notification = new AppleNotification().ForDeviceToken(data.DeviceToken);

            if (!string.IsNullOrWhiteSpace(data.Alert))
                notification = notification.WithAlert(data.Alert);

            if (data.Badge != null)
                notification.WithBadge(data.Badge.Value);

            if (!string.IsNullOrWhiteSpace(data.Sound))
                notification.WithSound(data.Sound);

            broker.QueueNotification(notification);
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
