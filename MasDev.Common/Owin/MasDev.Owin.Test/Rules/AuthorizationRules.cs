using MasDev.Common.Owin.Rules;


namespace MasDev.Common.Owin
{
	public class AuthorizationRules : MasDev.Common.Owin.Rules.AuthorizationRules
	{
		public AuthorizationRules ()
		{
			WhenMatches ("/user/{id}").WithMethods (HttpMethod.Get);
		}
	}
}

