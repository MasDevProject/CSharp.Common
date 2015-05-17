using MasDev.Services.Middlewares;
using MasDev.Services.Test;

namespace MasDev.Services.Test
{
	public class AuthorizationRules : BaseAuthorizationRules
	{
		public AuthorizationRules ()
		{
			WhenMatches (UserEndpoints.Resource).WithMethods (HttpMethod.Delete, HttpMethod.Get, HttpMethod.Put);
		}
	}
}