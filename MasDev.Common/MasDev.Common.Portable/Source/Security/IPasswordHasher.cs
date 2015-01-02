using System;

namespace MasDev.Security
{
	public interface IPasswordHasher
	{
		bool IsPasswordValid (byte[] clearPassword, IHashedPassword password);

		IHashedPassword HashPassword (byte[] clearPassword);
	}
}

