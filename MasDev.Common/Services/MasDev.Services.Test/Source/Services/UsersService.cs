using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;
using MasDev.Services.Test.Data;

namespace MasDev.Services.Test.Services
{
	public class UsersService : CrudService<UserDto, User, IUsersRepository>
	{
		public UsersService (IContext context) : base (context)
		{
		}
	}
}

