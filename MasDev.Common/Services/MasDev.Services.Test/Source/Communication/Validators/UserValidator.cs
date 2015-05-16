using MasDev.Services.Modeling;
using MasDev.Services.Test.Communication;
using System;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public class UserValidator : Validator<UserDto>
	{
		protected override void Validate (UserDto dto, IIdentityContext context)
		{
			if (dto == null)
				throw new ArgumentNullException ("dto");
			
			if (string.IsNullOrEmpty (dto.Username))
				throw new ArgumentException ("Username");

			var identity = context.Identity;
			if (dto.Id > 0 && !(identity.Roles == Roles.Admin || dto.Id == identity.Id))
				throw new Exception ("Not authorized");
		}
	}
}

