using System;
using System.Security.Cryptography;
using MasDev.Extensions;
using MasDev.Utils;


namespace MasDev.Security
{
	public class SHA256PasswordHasher : IPasswordHasher
	{
		const int _saltSize = 25;



		public bool IsPasswordValid (byte[] clearPassword, IHashedPassword password)
		{
			var hash = GenerateSaltedHash (clearPassword, password.PasswordSalt);
			return CompareByteArrays (hash, password.PasswordHash);
		}



		public IHashedPassword HashPassword (byte[] clearPassword)
		{
			var salt = GenerateSalt ();
			var hash = GenerateSaltedHash (clearPassword, salt);
			return new BaseHashedPassword {
				PasswordSalt = salt,
				PasswordHash = hash
			};
		}



		public string HashPassword (string clearText, string password)
		{
			return StringUtils.GetString (GenerateSaltedHash (clearText.AsByteArray (), password.AsByteArray ()));
		}



		static byte[] GenerateSalt ()
		{
			using (var generator = RandomNumberGenerator.Create ()) {
				var bytes = new byte[_saltSize];
				generator.GetNonZeroBytes (bytes);
				return bytes;
			}
		}



		static byte[] GenerateSaltedHash (byte[] plainText, byte[] salt)
		{
			var algorithm = new SHA256Managed ();

			var plainTextWithSaltBytes = 
				new byte[plainText.Length + salt.Length];

			for (int i = 0; i < plainText.Length; i++)
				plainTextWithSaltBytes [i] = plainText [i];

			for (int i = 0; i < salt.Length; i++)
				plainTextWithSaltBytes [plainText.Length + i] = salt [i];

			return algorithm.ComputeHash (plainTextWithSaltBytes);            
		}



		static bool CompareByteArrays (byte[] array1, byte[] array2)
		{
			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; i++) {
				if (array1 [i] != array2 [i])
					return false;
			}
			return true;
		}

	}
}

