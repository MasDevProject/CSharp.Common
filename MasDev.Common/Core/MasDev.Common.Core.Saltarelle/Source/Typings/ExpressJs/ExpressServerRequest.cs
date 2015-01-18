using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    [Imported]
    [IgnoreNamespace]
    [ModuleName("express")]
    public class ExpressServerRequest
    {

        private ExpressServerRequest()
        {
        }

        [IntrinsicProperty]
        public string[] AcceptedCharsets
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string[] AcceptedLanguages
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public JsDictionary<string, string> Body
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public JsDictionary<string, string> Cookies
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string Host
        {
            get
            {
                return null;
            }
        }
        [IntrinsicProperty]
        [ScriptName("method")]
        public string Method
        {
            get
            {
                return null;
            }
        }
        [IntrinsicProperty]
        [ScriptName("ip")]
        public string IPAddress
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        [ScriptName("fresh")]
        public bool IsFresh
        {
            get
            {
                return false;
            }
        }

        [IntrinsicProperty]
        [ScriptName("stale")]
        public bool IsStale
        {
            get
            {
                return false;
            }
        }

        [IntrinsicProperty]
        [ScriptName("secure")]
        public bool IsSecure
        {
            get
            {
                return false;
            }
        }

        [IntrinsicProperty]
        [ScriptName("params")]
        public JsDictionary<string, string> Parameters
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string Path
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string Protocol
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string OriginalUrl
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public JsDictionary<string, string> Query
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public ExpressRoute Route
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public object Session
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
        public JsDictionary<string, string> SignedCookies
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string[] Subdomains
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public object User
        {
            get
            {
                return null;
            }
        }

        public bool AcceptsCharset(string charset)
        {
            return false;
        }

        [ScriptName("accepts")]
        public string AcceptsContentType(string type)
        {
            return null;
        }

        [ScriptName("accepts")]
        public string AcceptsContentType(string[] types)
        {
            return null;
        }

        public bool AcceptsLanguage(string language)
        {
            return false;
        }

        [ScriptName("get")]
        public string GetHeader(string name)
        {
            return null;
        }

        [ScriptName("param")]
        public string GetParameter(string name)
        {
            return null;
        }

        [ScriptName("is")]
        public string IsContentType(string type)
        {
            return null;
        }
    }
}
