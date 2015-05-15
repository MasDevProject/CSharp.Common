using MasDev.Services.Modeling;
using MasDev.Services.Test.Communication;
using System;

namespace MasDev.Services.Test
{
	public class UserValidator : Validator<UserDto>
	{
		protected override void Validate (UserDto dto, IContext context)
		{
			if (dto == null)
				throw new ArgumentNullException ("dto");
			
			if (string.IsNullOrEmpty (dto.Username))
				throw new ArgumentException ("Username");
		}
	}
}

