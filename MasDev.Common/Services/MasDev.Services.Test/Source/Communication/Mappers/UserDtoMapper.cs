using MasDev.Services.Test.Models;
using MasDev.Services.Modeling;
using AutoMapper;
using MasDev.Common;


namespace MasDev.Services.Test.Communication
{
	public class UserDtoMapper : CommunicationMapper<UserDto, User>
	{
		protected override User Map (UserDto dto, IIdentityContext context)
		{
			return Mapper.DynamicMap<User> (dto);
		}

		protected override UserDto Map (User model, IIdentityContext context)
		{
			var dto = Mapper.DynamicMap<UserDto> (model);

			if (context != null && context.Identity != null && context.Identity.Roles != Roles.Admin)
				dto.Password = null;

			return dto;
		}
	}
}

