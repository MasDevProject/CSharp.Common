using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;
using MasDev.Services.Test.Data;
using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System.Linq;
using MasDev.Data;
using MasDev.Services.Auth;
using System;
using MasDev.Common;

namespace MasDev.Services.Test.Services
{
	public interface IUserService : ICrudService<UserDto>
	{
		Task<LoginResult<UserDto>> LoginAsync (string username, string password, IIdentityContext context);
	}

	public class UsersService : CrudService<UserDto, User, IUsersRepository>, IUserService
	{
		public async Task<LoginResult<UserDto>> LoginAsync (string username, string password, IIdentityContext context)
		{
			var user = await Repository.Query.Where (u => u.Username == username && u.Password == password).SingleOrDefaultAsync ();
			if (user == null)
				throw new InputException ();

			var expirationUtc = DateTime.UtcNow.AddMonths (1);
			var userDto = await MapAsync (user, context);
			var accessToken = AuthorizationManager.Current.GenerateAccessToken (user.Id, user.Roles, expirationUtc);
			return new LoginResult<UserDto> {
				AccessToken = accessToken,
				AccessTokenExpirationUtc = expirationUtc,
				Identity = userDto
			};
		}
	}
}

