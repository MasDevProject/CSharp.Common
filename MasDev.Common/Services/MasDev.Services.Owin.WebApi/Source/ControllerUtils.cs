using MasDev.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MasDev.Extensions;

namespace MasDev.Services.Owin.WebApi.Source
{
    static class ControllerUtils
    {
        static HttpResponseMessage AddHeaders(HttpResponseMessage response, Dictionary<string, IEnumerable<string>> responseHeaders)
        {
            if (responseHeaders == null || responseHeaders.Count == 0)
                return response;

            Debug.WriteLine("Additional headers:");
            foreach (var header in responseHeaders)
            {
                Debug.Write("\t" + header.Key + " : ");
                foreach (var value in header.Value)
                    Debug.Write(value + "; ");
                Debug.Write("\n");
                response.Headers.Add(header.Key, header.Value);
            }
            Debug.WriteLine(string.Empty);

            return response;
        }



        public static DynamicDictionary ParseParameters(HttpContent content)
        {
            if (content.IsMimeMultipartContent())
                throw new ArgumentException("Request body must be form urlencoded");

            var rawTask = content.ReadAsStringAsync();
            rawTask.Wait();

            if (rawTask.Exception != null)
                throw rawTask.Exception;

            var dict = new DynamicDictionary();

            var raw = ParseBodyParameters(rawTask.Result);
            raw.AllKeys
                .SelectMany(raw.GetValues, (k, v) => new { Key = k, Value = v })
                .ForEach(el => dict.Add(el.Key, el.Value));

            return dict;
        }



        public static NameValueCollection ParseBodyParameters(string body)
        {
            return HttpUtility.ParseQueryString(body);
        }



        public static async Task<Tuple<string, string>> RequestBodyAsMimedStream(HttpRequestMessage request)
        {
            var tempFileUrl = Path.GetTempPath() + "/" + GuidGenerator.Generate() + ".tmp";
            using (var stream = new FileStream(tempFileUrl, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                await request.Content.CopyToAsync(stream);

            var mime = request.Content.Headers.ContentType.MediaType;

            return Tuple.Create(tempFileUrl, mime);
        }
    }
}
