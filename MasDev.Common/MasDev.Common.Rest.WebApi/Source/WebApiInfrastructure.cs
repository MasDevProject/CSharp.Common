using System.Web.Http;
using Newtonsoft.Json;
using System;
using MasDev.Common.Utils;
using MasDev.Common.Newtonsoft.ContractResolvers;
using Newtonsoft.Json.Serialization;


namespace MasDev.Common.Rest.WebApi
{
	public static class WebApiInfrastructure
	{
		public static Action<HttpConfiguration> GetRegistration (WebApiInfrastructureConfiguration config)
		{
			Assert.NotNull (config);
			return configuration => {
				config.AttributeRoutesMapper (configuration);
				configuration.Filters.Add (config.ExceptionInterceptor == null ?
					new ExceptionLoggerFilter () :
					new ExceptionLoggerFilter (config.ExceptionInterceptor)
				);
				configuration.Filters.Add (new ActionFilter ());
				configuration.MessageHandlers.Add (new ResponseMarshallerDelegatingHandler ());

				configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
				var jsonSettings = configuration.Formatters.JsonFormatter.SerializerSettings;
				jsonSettings.ContractResolver = config.ContractResolver;
				jsonSettings.NullValueHandling = NullValueHandling.Ignore;
				jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			};
		}
	}





	public class WebApiInfrastructureConfiguration
	{
		public Action<HttpConfiguration> AttributeRoutesMapper { get; private set; }


		public Action<Exception> ExceptionInterceptor { get; private set; }


		public IContractResolver ContractResolver { get; private set; }



		public WebApiInfrastructureConfiguration (IContractResolver contractResolver, Action<HttpConfiguration> attributeRoutesMapper)
		{
			AttributeRoutesMapper = attributeRoutesMapper;
			ContractResolver = contractResolver;
		}



		public WebApiInfrastructureConfiguration (IContractResolver contractResolver, Action<HttpConfiguration> attributeRoutesMapper, Action<Exception> exceptionInterceptor) : this (contractResolver, attributeRoutesMapper)
		{
			ExceptionInterceptor = exceptionInterceptor;
		}
	}
}

