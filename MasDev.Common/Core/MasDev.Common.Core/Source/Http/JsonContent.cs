using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace MasDev.IO.Http
{
	public class JsonContent<T> : StringContent
	{
		const string _contentType = "application/json";
		public readonly T Content;

		public JsonContent (T obj) : base (JsonConvert.SerializeObject (obj), Encoding.UTF8, _contentType)
		{
			Content = obj;
		}
	}

	public static class JsonContent 
	{
		public static T Deserialize<T> (string content)
		{
			return JsonConvert.DeserializeObject<T> (content);
		}
	}
}

