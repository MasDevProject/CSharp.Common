using MasDev.Owin.Auth;

namespace MasDev.Owin.Test
{
	public class AuthorizationRules : MasDev.Owin.Auth.AuthorizationRules
	{
		public AuthorizationRules ()
		{
			WhenMatches ("/user/{id}").WithMethods (HttpMethod.Get);
		}
	}
}

