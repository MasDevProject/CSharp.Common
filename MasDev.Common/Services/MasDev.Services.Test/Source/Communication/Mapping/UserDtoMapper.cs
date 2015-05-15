using MasDev.Services.Test.Models;
using MasDev.Services.Modeling;
using System;
using AutoMapper;


namespace MasDev.Services.Test.Communication
{
	public class UserDtoMapper : DtoMapper<UserDto, User>
	{

		protected override void Validate (UserDto dto, Identity identity)
		{
			if (dto == null)
				throw new ArgumentNullException ("dto");

			if (string.IsNullOrEmpty (dto.Username))
				throw new ArgumentException ("Username");
		}

		protected override User MapInternal (UserDto dto, Identity identity)
		{
			return Mapper.DynamicMap<User> (dto);
		}

		public override UserDto Map (User model, Identity identity, int? scope = null)
		{
			var dto = Mapper.DynamicMap<UserDto> (model);

			if (identity.Roles != Roles.Admin)
				dto.Password = null;

			return dto;
		}
	}
}

