using Microsoft.Owin;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;


namespace MasDev.Services.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<BasePathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (OwinMiddleware next, BasePathMappingRules rules) : base (next, rules)
		{
		}

		protected PathMappingMiddleware (OwinMiddleware next, string configFilePath) : base (next, LoadRules (configFilePath))
		{
		}

		static BasePathMappingRules LoadRules (string configFilePath)
		{
			var rules = new BasePathMappingRules ();
			var text = File.ReadAllText (configFilePath);
			var root = JObject.Parse (text);

			foreach (var pair in root) {
				var pairValue = pair.Value;

				if (pairValue.Type != JTokenType.String)
					throw new NotSupportedException ("Json value must be an url");

				rules.WhenMatches (pair.Key).MapTo (pairValue.Value<string> ());
			}

			return rules;
		}
	}
}

