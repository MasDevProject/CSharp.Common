using MasDev.Services.Middlewares;
using MasDev.Services.Test;
using MasDev.Services.Test.Models;


namespace MasDev.Services.Test
{
	public class AuthorizationRules : BaseAuthorizationRules
	{
		public AuthorizationRules ()
		{
			WhenMatches (UserEndpoints.Delete).WithMethods (HttpMethod.Delete).RequireAtLeast (Roles.Admin);
			WhenMatches (UserEndpoints.Update).WithMethods (HttpMethod.Put);
		}
	}
}

