using MasDev.Services.Middlewares;


namespace MasDev.Services
{
	public class AuthorizationRules : MasDev.Services.Middlewares.AuthorizationRules
	{
		public AuthorizationRules ()
		{
			WhenMatches ("/user/{id}").WithMethods (HttpMethod.Get);
		}
	}
}

