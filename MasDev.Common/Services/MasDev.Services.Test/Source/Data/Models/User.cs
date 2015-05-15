using MasDev.Data;


namespace MasDev.Services.Test.Models
{
	public class User : IModel
	{
		public virtual int Id { get; set; }

		public virtual int Roles { get; set; }

		public virtual string Username { get; set; }

		public virtual string Password { get; set; }
	}
}

