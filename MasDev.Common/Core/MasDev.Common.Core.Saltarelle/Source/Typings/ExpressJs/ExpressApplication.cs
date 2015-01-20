using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NodeJS.HttpModule;

namespace NodeExpress
{
	[Imported]
	[ModuleName ("express")]
	[IgnoreNamespace]
	public class ExpressApplication
	{

		public JsDictionary<string, object> Locals { get; set; }

		public JsDictionary<string, List<ExpressRoute>> Routes { get; set; }


		[ScriptName ("locals")]
		public void AddLocals (Dictionary<string, object> values)
		{
		}

		public ExpressApplication All (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication All (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}



		public ExpressApplication Configure (Action callback)
		{
			return null;
		}

		public ExpressApplication Configure (string environmentName, Action callback)
		{
			return null;
		}

		public ExpressApplication Delete (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Delete (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}

      

		[ScriptName ("disable")]
		public ExpressApplication DisableSetting (string name)
		{
			return null;
		}

		[ScriptName ("disable")]
		public ExpressApplication DisableSetting (ExpressSettings name)
		{
			return null;
		}

		[ScriptName ("enable")]
		public ExpressApplication EnableSetting (string name)
		{
			return null;
		}

		[ScriptName ("enable")]
		public ExpressApplication EnableSetting (ExpressSettings name)
		{
			return null;
		}

		public ExpressApplication Engine (string extension, ExpressTemplateEngine engine)
		{
			return null;
		}

		public ExpressApplication Error (ExpressErrorHandler handler)
		{
			return null;
		}

		public ExpressApplication Error (ExpressChainedErrorHandler chainedHandler)
		{
			return null;
		}

		public ExpressApplication Get (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Get (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}



		[ScriptName ("get")]
		public string GetSetting (string name)
		{
			return null;
		}

		[ScriptName ("get")]
		public string GetSetting (ExpressSettings name)
		{
			return null;
		}

		public ExpressApplication Head (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Head (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}

    

		[ScriptName ("disabled")]
		public bool IsSeSettingDisabled (string name)
		{
			return false;
		}

		[ScriptName ("disabled")]
		public bool IsSeSettingDisabled (ExpressSettings name)
		{
			return false;
		}

		[ScriptName ("enabled")]
		public bool IsSeSettingEnabled (string name)
		{
			return false;
		}

		[ScriptName ("enabled")]
		public bool IsSeSettingEnabled (ExpressSettings name)
		{
			return false;
		}

		public dynamic Listen (int port, Action callback)
		{
			return null;
		}

		public void Listen (int port)
		{
		}

		public void Listen (int port, string hostName)
		{
		}

		public void Listen (int port, string hostName, int backlog)
		{
		}

		public ExpressApplication Param (string parameter, ExpressParameterHandler handler)
		{
			return null;
		}

		public ExpressApplication Options (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Options (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}

   

		public ExpressApplication Post (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Post (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}

 

		public ExpressApplication Put (string path, ExpressHandler handler)
		{
			return null;
		}

		public ExpressApplication Put (string path, ExpressChainedHandler[] chainedHandlers, ExpressHandler handler)
		{
			return null;
		}

   

		public void Render (string viewName, Action<string> callback)
		{
		}

		public void Render (string viewName, Dictionary<string, object> data, Action<string> callback)
		{
		}

		[ScriptName ("set")]
		public void SetSetting (string name, string value)
		{
		}

		[ScriptName ("set")]
		public void SetSetting (ExpressSettings name, string value)
		{
		}

		/*[ScriptSkip]
        public HttpListener ToHttpListener()
        {
            return null;
        }*/

		public ExpressApplication Use (ExpressMiddleware middleware)
		{
			return null;
		}

		public ExpressApplication Use (ExplicitExpressMiddleware middleware)
		{
			return null;
		}

		public ExpressApplication Use (string path, ExpressMiddleware middleware)
		{
			return null;
		}
	}
}
