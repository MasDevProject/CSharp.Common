using System;
using System.Runtime.CompilerServices;

namespace NodeExpress
{
	[Imported, ModuleName ("express"), ScriptName ("")]
	public static class Express
	{
		public static readonly string ModuleName = "express";

		[InlineCode ("{$NodeExpress.Express}()")]
		public static ExpressApplication Application ()
		{
			return null;
		}

		/* [InlineCode("{@arg0}")]
        public static void InlineMe(string arg0)
        {

        }*/
		static Express ()
		{

		}

		public static ExpressMiddleware BasicAuth (string userName, string password)
		{
			return null;
		}

		public static ExpressMiddleware BasicAuth (Func<string, string, bool> verificationCallback)
		{
			return null;
		}

		public static ExpressMiddleware BasicAuth (Func<string, string, Action<Exception, object>> verificationCallback)
		{
			return null;
		}

		public static ExpressMiddleware BodyParser ()
		{
			return null;
		}

		public static ExpressMiddleware Compress ()
		{
			return null;
		}

		public static ExpressMiddleware CookieParser ()
		{
			return null;
		}

		public static ExpressMiddleware CookieParser (string secret)
		{
			return null;
		}

		public static ExpressMiddleware CookieSession ()
		{
			return null;
		}

		[ScriptName ("csrf")]
		public static ExpressMiddleware CSRF ()
		{
			return null;
		}

		public static ExpressMiddleware Directory ()
		{
			return null;
		}

		public static ExpressMiddleware Directory (string path)
		{
			return null;
		}

		public static ExpressMiddleware ErrorHandler ()
		{
			return null;
		}

		[ScriptName ("json")]
		public static ExpressMiddleware JSON ()
		{
			return null;
		}

		public static ExpressMiddleware Logger ()
		{
			return null;
		}

		[ScriptName ("multipart")]
		public static ExpressMiddleware MultiPart ()
		{
			return null;
		}

		public static ExpressMiddleware Static (string path)
		{
			return null;
		}

		public static ExpressMiddleware Static (string path, object options)
		{
			return null;
		}

		[ScriptName ("urlencoded")]
		public static ExpressMiddleware UrlEncoded ()
		{
			return null;
		}
	}
}
