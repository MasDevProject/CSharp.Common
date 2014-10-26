
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MasDev.Common.Http;
using System.Net;
using MasDev.Common.Exceptions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using MasDev.Common.Utils;


namespace MasDev.Common.Utils.GoogleServices.CloudMessaging
{
	public class CloudMessagingService
	{
		const string _url = "https://android.googleapis.com/gcm/send";
		const string _contentType = "application/json";
		const string _authorization = "key=";



		public async Task<GcmResult> SendPushAsync (string serverApiKey, GcmPush push)
		{
			using (var client = new HttpClient ())
			{
				var json = JsonConvert.SerializeObject (new GcmPushCCased (push));

				var httpContent = new StringContent (json);
				httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse (_contentType);
				var request = new HttpRequestMessage (HttpMethod.Post, _url);
				request.Headers.TryAddWithoutValidation (AuthorizationHeader.Name, _authorization + serverApiKey);
				request.Content = httpContent;

				var response = await client.SendAsync (request);
				if (response.StatusCode != HttpStatusCode.OK)
					throw new HttpException (response.StatusCode);

				return JsonConvert.DeserializeObject<GcmResult> (await response.Content.ReadAsStringAsync ());
			}
		}
	}







	#region utility classes
	class GcmPushCCased
	{
		public GcmPushCCased (GcmPush push)
		{
			var ids = Assert.NotNull (push).ClientIds.ToList ();
			registration_ids = Assert.NotNullOrEmpty (ids).ToArray ();
			data = push.Data;
			collapse_key = push.CollapseKey;
			delay_while_idle = push.DelayWhileIdle ? (bool?)push.DelayWhileIdle : null;
			time_to_live = push.TimeToLive != null ? (long?)push.TimeToLive.Value.TotalSeconds : null;
			restricted_package_name = push.RestrictedPackageName;

		}



		public string[] registration_ids { get; private set; }



		public string collapse_key { get; private set; }



		public dynamic data { get; private set; }



		public bool? delay_while_idle { get; private set; }



		public long? time_to_live { get; private set; }



		public string restricted_package_name { get; private set; }
	}





	public class GcmResult
	{
		public long MulticastId { get; private set; }



		public int Success { get; set; }



		public int Failure { get ; set; }



		public long CanonicalIds { get; set; }



		public dynamic[] Results { get; set; }
	}





	public class GcmPush
	{
		public IEnumerable<string> ClientIds { get; set; }



		public string CollapseKey { get; set; }



		public dynamic Data { get; set; }



		public bool DelayWhileIdle { get; set; }



		public TimeSpan? TimeToLive { get; set; }



		public string RestrictedPackageName { get; set; }
	}
	#endregion
}

