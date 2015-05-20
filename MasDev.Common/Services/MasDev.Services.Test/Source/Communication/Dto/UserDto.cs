using MasDev.Common;


namespace MasDev.Services.Test.Communication
{
	public class UserDto : IEntity
	{
		public int Id { get; set; }

		public int Roles { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}

