using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    [Imported]
    [IgnoreNamespace]
    [ModuleName("express")]
    public class ExpressServerResponse
    {

        private ExpressServerResponse()
        {
        }

        [IntrinsicProperty]
        public string Charset
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        [IntrinsicProperty]
        public JsDictionary<string, object> Locals
        {
            get
            {
                return null;
            }
        }

        [ScriptName("locals")]
        public void AddLocals(JsDictionary<string, object> values)
        {
        }

        public ExpressServerResponse ClearCookie(string header)
        {
            return null;
        }

        public ExpressServerResponse ClearCookie(string header, object options)
        {
            return null;
        }

        [ScriptName("get")]
        public string GetHeader(string header)
        {
            return null;
        }

        public void Redirect(string url)
        {
        }

        public void Redirect(HttpStatusCode statusCode, string url)
        {
        }

        public void Render(string viewName, Action<string> callback)
        {
        }

        public void Render(string viewName, JsDictionary<string, object> data, Action<string> callback)
        {
        }
            public void Send(object data)
        {
        }

        public void Send(HttpStatusCode statusCode)
        {
        }

        public void Send(HttpStatusCode statusCode, object data)
        {
        }

        public void SendFile(string path)
        {
        }

        public void SendFile(string path, object options)
        {
        }

        public void SendFile(string path, object options, Action callback)
        {
        }

        [ScriptName("download")]
        public void SendFileAttachment(string path)
        {
        }

        [ScriptName("download")]
        public void SendFileAttachment(string path, string fileName)
        {
        }

        [ScriptName("download")]
        public void SendFileAttachment(string path, string fileName, Action callback)
        {
        }

        [ScriptName("json")]
        public void SendJson(object data)
        {
        }

        [ScriptName("json")]
        public void SendJson(HttpStatusCode statusCode, object data)
        {
        }

        [ScriptName("jsonp")]
        public void SendJsonP(object data)
        {
        }

        [ScriptName("jsonp")]
        public void SendJsonP(HttpStatusCode statusCode, object data)
        {
        }

        [ScriptName("attachment")]
        public ExpressServerResponse SetAttachment()
        {
            return null;
        }

        [ScriptName("attachment")]
        public ExpressServerResponse SetAttachment(string fileName)
        {
            return null;
        }

        [ScriptName("cookie")]
        public ExpressServerResponse SetCookie(string header, string value)
        {
            return null;
        }

        [ScriptName("cookie")]
        public ExpressServerResponse SetCookie(string header, string value, ExpressCookieOptions options)
        {
            return null;
        }

        [ScriptName("set")]
        public ExpressServerResponse SetHeader(string header, string value)
        {
            return null;
        }

        [ScriptName("set")]
        public ExpressServerResponse SetHeaders(JsDictionary<string, string> headers)
        {
            return null;
        }

        [ScriptName("links")]
        public ExpressServerResponse SetLinks(JsDictionary<string, string> links)
        {
            return null;
        }

        [ScriptName("status")]
        public ExpressServerResponse SetStatus(HttpStatusCode code)
        {
            return null;
        }

        [ScriptName("type")]
        public ExpressServerResponse SetType(string type)
        {
            return null;
        }
    }
}
