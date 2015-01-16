using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace MasDev.Utils
{
	public static class ConnectionUtils
	{
		public static async Task<bool> CheckConnection (string url)
		{
			try {
				using (var client = new HttpClient ()) {
					client.Timeout = TimeSpan.FromSeconds (10);
					await client.GetStringAsync (url);
					return true;
				}
			} catch (Exception) {
				return false;
			}
		}
	}
}

