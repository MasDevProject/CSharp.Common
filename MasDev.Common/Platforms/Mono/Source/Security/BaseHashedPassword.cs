using System;

namespace MasDev.Security
{
	public class BaseHashedPassword : IHashedPassword
	{
		public byte[] PasswordHash { get; set; }

		public byte[] PasswordSalt { get; set; }
	}
}

