using System.Web.Http;
using Newtonsoft.Json;
using System;
using MasDev.Common.Utils;


namespace MasDev.Common.Rest.WebApi
{
	public static class WebApiInfrastructure
	{
		public static Action<HttpConfiguration> GetRegistration (WebApiInfrastructureConfiguration config)
		{
			Assert.NotNull (config);
			return configuration =>
			{
				config.AttributeRoutesMapper (configuration);
				configuration.Filters.Add (config.ExceptionInterceptor == null ?
					new ExceptionLoggerFilter () :
					new ExceptionLoggerFilter (config.ExceptionInterceptor)
				);
				configuration.Filters.Add (new ActionFilter ());
				configuration.MessageHandlers.Add (new ResponseMarshallerDelegatingHandler ());

				configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
				var jsonSettings = configuration.Formatters.JsonFormatter.SerializerSettings;
				jsonSettings.ContractResolver = new IgnoreEmptyContractResolver ();
				jsonSettings.NullValueHandling = NullValueHandling.Ignore;
			};
		}
	}





	public class WebApiInfrastructureConfiguration
	{
		public Action<HttpConfiguration> AttributeRoutesMapper { get; private set; }



		public Action<Exception> ExceptionInterceptor { get; private set; }



		public WebApiInfrastructureConfiguration (Action<HttpConfiguration> attributeRoutesMapper)
		{
			AttributeRoutesMapper = attributeRoutesMapper;
		}



		public WebApiInfrastructureConfiguration (Action<HttpConfiguration> attributeRoutesMapper, Action<Exception> exceptionInterceptor) : this (attributeRoutesMapper)
		{
			ExceptionInterceptor = exceptionInterceptor;
		}
	}
}

