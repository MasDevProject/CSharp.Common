using MasDev.Data;


namespace MasDev.Services.Test.Models
{
	public class User : IModel
	{
		public int Id { get; set; }

		public int Roles { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}

