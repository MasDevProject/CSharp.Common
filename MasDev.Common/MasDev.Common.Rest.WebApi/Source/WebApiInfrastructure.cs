using System.Web.Http;
using Newtonsoft.Json;
using System;


namespace MasDev.Common.Rest.WebApi
{
    public delegate void MapHttpAttributeRoutes(HttpConfiguration config);
	public static class WebApiInfrastructure
	{
		public static Action<HttpConfiguration> GetRegistration (MapHttpAttributeRoutes mapper)
		{
            return configuration =>
                {
                    mapper(configuration);
                    configuration.Filters.Add(new ExceptionLoggerFilter());
                    configuration.Filters.Add(new ActionFilter());
                    configuration.MessageHandlers.Add(new ResponseMarshallerDelegatingHandler());

                    configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                    var jsonSettings = configuration.Formatters.JsonFormatter.SerializerSettings;
                    jsonSettings.ContractResolver = new IgnoreEmptyContractResolver();
                    jsonSettings.NullValueHandling = NullValueHandling.Ignore;
                };
		}
	}
}

