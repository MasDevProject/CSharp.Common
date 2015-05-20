using MasDev.Common;
using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public class UserDtoAccessValidator : EntityAccessValidator<UserDto>
	{
		protected override void Validate (int id, IIdentityContext context)
		{
			if (context == null)
				throw new UnauthorizedException ();

			var identity = context.Identity;
			if (identity == null)
				throw new UnauthorizedException ();
			
			if (!(identity.Roles == Roles.Admin || identity.Id != id))
				throw new UnauthorizedException ();
		}
	}
}

