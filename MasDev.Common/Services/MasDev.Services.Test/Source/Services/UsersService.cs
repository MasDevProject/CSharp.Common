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
		Task<LoginResult<UserDto>> LoginAsync (string username, string password);

		Task<UserDto> MeAsync ();
	}

	public class UsersService : BaseCrudService<UserDto, User, IUsersRepository>, IUserService
	{
		public async Task<LoginResult<UserDto>> LoginAsync (string username, string password)
		{
			var user = await Repository.Query.Where (u => u.Username == username && u.Password == password).SingleOrDefaultAsync ();
			if (user == null)
				throw new InputException ();

			var expirationUtc = DateTime.UtcNow.AddMonths (1);
			var userDto = await MapAsync (user);
			var accessToken = AuthorizationManager.Current.GenerateAccessToken (user.Id, user.Roles, expirationUtc);
			return new LoginResult<UserDto> {
				AccessToken = accessToken,
				AccessTokenExpirationUtc = expirationUtc,
				Identity = userDto
			};
		}

		public async Task<UserDto> MeAsync ()
		{
			await AuthorizeAsync ();

			var user = await Repository.ReadAsync (IdentityContext.Identity.Id);
			ThrowIfNotFound (user);
			
			return await MapAsync (user);
		}
	}
}

