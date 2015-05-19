using System.Web.Http;
using Newtonsoft.Json;
using MasDev.Patterns.Injection;
using MasDev.Services.Owin.WebApi;
using Newtonsoft.Json.Serialization;

namespace MasDev.Services
{
	partial class Startup
	{
		public static HttpConfiguration ConfigureWebApi ()
		{
			var config = new HttpConfiguration ();
			var jsonFormatter = config.Formatters.JsonFormatter;
			jsonFormatter.Indent = false;
			jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			jsonFormatter.SerializerSettings.ContractResolver = Injector.Resolve<IContractResolver> ();
			jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

			config.Filters.Add (new ExceptionFilter ());
			config.MessageHandlers.Add (new SerializerDelegatingHandler ());

			config.MapHttpAttributeRoutes ();
			return config;
		}
	}
}

