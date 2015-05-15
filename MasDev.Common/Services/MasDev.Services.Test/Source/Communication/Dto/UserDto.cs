using MasDev.Services.Modeling;


namespace MasDev.Services.Test.Communication
{
	public class UserDto : IDto
	{
		public int Id { get; set; }

		public int Roles { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}

