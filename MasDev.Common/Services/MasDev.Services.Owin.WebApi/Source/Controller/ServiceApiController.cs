using System.Web.Http;
using MasDev.Patterns.Injection;
using System.Collections.Generic;
using System;
using MasDev.Services.Owin.WebApi.Source;
using System.Threading.Tasks;
using HttpMultipartParser;

namespace MasDev.Services.Owin.WebApi
{
	public class ServiceApiController : ApiController
	{
        Dictionary<string, IEnumerable<string>> _responseHeaders = new Dictionary<string, IEnumerable<string>>();
        readonly Lazy<DynamicDictionary> _lazyBodyParameters;

        public ServiceApiController()
        {
            _lazyBodyParameters = new Lazy<DynamicDictionary>(() => ControllerUtils.ParseParameters(Request.Content));
        }

        public DynamicDictionary BodyParameters { get { return _lazyBodyParameters.Value; } }

        public Dictionary<string, IEnumerable<string>> RequestHeaders
        {
            get
            {
                var dict = new Dictionary<string, IEnumerable<string>>();
                foreach (var entry in Request.Headers)
                    dict.Add(entry.Key, entry.Value);
                return dict;
            }
        }

        public Dictionary<string, IEnumerable<string>> ResponseHeaders
        {
            set
            {
                _responseHeaders = value;
            }

            get
            {
                return _responseHeaders;
            }
        }

        public string RequestHost
        {
            get
            {
                var uri = Request.RequestUri;
                var host = uri.Host;
                return uri.Scheme + "://" + host + ":" + uri.Port + "/";
            }
        }

        public async Task<MultipartContent> ReadMultipartContentAsync()
        {
            var parser = new MultipartFormDataParser(await Request.Content.ReadAsStreamAsync());

            var parsedParameters = parser.Parameters;
            var parameters = new Dictionary<string, string>();
            if (parsedParameters != null)
            {
                foreach (var param in parsedParameters)
                    parameters.Add(param.Key, param.Value.Data);
            }

            var parsedFiles = parser.Files;
            var files = new List<MultipartFile>();
            if (parsedFiles != null)
            {
                foreach (var file in parsedFiles)
                    files.Add(new MultipartFile(file.Data, file.Name, file.FileName, file.ContentType));
            }

            return new MultipartContent(new MultipartFormDataCollection(parameters), files);
        }
    }

	public class ServiceApiController<TService> : ServiceApiController where TService : class, IService
	{
		protected readonly TService Service;

		public ServiceApiController ()
		{
			Service = Injector.Resolve<TService> ();
		}
	}
}

