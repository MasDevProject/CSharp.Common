using MasDev.Services.Test.Communication;
using MasDev.Services.DataAccess;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public class UserDataAccessValidator : DataAccessValidator<UserDto, User>
	{
		protected override bool CanAccess (int id, IIdentityContext context)
		{			
			var identity = context.Identity;
			if (identity == null)
				return false;

			return identity.Id == id || identity.Roles == Roles.Admin;
		}
	}
}

