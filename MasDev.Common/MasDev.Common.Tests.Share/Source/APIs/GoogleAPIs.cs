using System.Net.Http;
using MasDev.Common.Extensions;
using System.Threading.Tasks;

namespace MasDev.Common.Share.Tests
{
	public static class GoogleAPIs
	{
		public static async Task<string> GetGoogleHomePage()
		{
			var client = new HttpClient();
			var result = await client.GetAsync("http://www.google.com");
			return await result.AsStringAsync();
		}
	}
}

