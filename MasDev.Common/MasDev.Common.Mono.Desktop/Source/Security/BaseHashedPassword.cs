using System;

namespace MasDev.Common.Security
{
	public class BaseHashedPassword : IHashedPassword
	{
		public byte[] PasswordHash { get; set; }

		public byte[] PasswordSalt { get; set; }
	}
}

