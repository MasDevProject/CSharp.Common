using System.Threading.Tasks;
using Android.Gms.Gcm.Iid;
using Android.Gms.Gcm;
using Android.Content;

namespace MasDev.Droid.Utils
{
	public static class GCMUtils
	{
		public static async Task<string> PerformRegistration (Context ctx, string senderId)
		{
			return await System.Threading.Tasks.Task.Run (() => {
				return InstanceID.GetInstance(ctx).GetToken(senderId, GoogleCloudMessaging.InstanceIdScope, null);
			});
		}
	}
}

