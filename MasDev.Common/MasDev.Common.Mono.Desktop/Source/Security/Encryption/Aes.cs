using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace MasDev.Common.Security.Encryption
{
	public class Aes : ISymmetricCrypto
	{
		const string HashAlgorithm = "SHA1";
		const string InitialVector = "BLRta73m*aze01xQ";
		const string Salt = "MasDev";
		const int PasswordIterations = 3000;
		const int KeySize = 256;



		public string Encrypt (string plainText, string password)
		{
			return Encrypt (plainText, password, Salt, PasswordIterations, InitialVector, KeySize);
		}



		public string Decrypt (string cypherText, string password)
		{
			return Decrypt (cypherText, password, Salt, PasswordIterations, InitialVector, KeySize);
		}

		public static string StaticEncrypt (string plainText, string password)
		{
			return Encrypt (plainText, password, Salt, PasswordIterations, InitialVector, KeySize);
		}



		public static string StaticDecrypt (string cypherText, string password)
		{
			return Decrypt (cypherText, password, Salt, PasswordIterations, InitialVector, KeySize);
		}

		public static string Encrypt (string plainText, string password, string salt, int passwordIterations, string initialVector, int keySize)
		{
			if (string.IsNullOrEmpty (plainText))
				return string.Empty;

			byte[] InitialVectorBytes = Encoding.ASCII.GetBytes (initialVector);
			byte[] SaltValueBytes = Encoding.ASCII.GetBytes (salt);
			byte[] PlainTextBytes = Encoding.UTF8.GetBytes (plainText);
			var DerivedPassword = new PasswordDeriveBytes (password, SaltValueBytes, HashAlgorithm, passwordIterations);
			byte[] KeyBytes = DerivedPassword.GetBytes (keySize / 8);
			var SymmetricKey = new RijndaelManaged ();
			SymmetricKey.Mode = CipherMode.CBC;
			byte[] CipherTextBytes = null;
			using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor (KeyBytes, InitialVectorBytes))
			using (var MemStream = new MemoryStream ())
			using (var CryptoStream = new CryptoStream (MemStream, Encryptor, CryptoStreamMode.Write)) {
				CryptoStream.Write (PlainTextBytes, 0, PlainTextBytes.Length);
				CryptoStream.FlushFinalBlock ();
				CipherTextBytes = MemStream.ToArray ();
				MemStream.Close ();
				CryptoStream.Close ();
			}
			SymmetricKey.Clear ();
			return Convert.ToBase64String (CipherTextBytes);
		}



		public static string Decrypt (string cipherText, string password, string salt, int passwordIterations, string initialVector, int keySize)
		{
			if (string.IsNullOrEmpty (cipherText))
				return string.Empty;

			byte[] InitialVectorBytes = Encoding.ASCII.GetBytes (initialVector);
			byte[] SaltValueBytes = Encoding.ASCII.GetBytes (salt);
			byte[] CipherTextBytes = Convert.FromBase64String (cipherText);
			var DerivedPassword = new PasswordDeriveBytes (password, SaltValueBytes, HashAlgorithm, passwordIterations);
			byte[] KeyBytes = DerivedPassword.GetBytes (keySize / 8);
			var SymmetricKey = new RijndaelManaged ();
			SymmetricKey.Mode = CipherMode.CBC;
			var PlainTextBytes = new byte[CipherTextBytes.Length];
			int ByteCount = 0;
			using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor (KeyBytes, InitialVectorBytes))
			using (var MemStream = new MemoryStream (CipherTextBytes))
			using (var CryptoStream = new CryptoStream (MemStream, Decryptor, CryptoStreamMode.Read)) {

				ByteCount = CryptoStream.Read (PlainTextBytes, 0, PlainTextBytes.Length);
				MemStream.Close ();
				CryptoStream.Close ();
			}
			SymmetricKey.Clear ();
			return Encoding.UTF8.GetString (PlainTextBytes, 0, ByteCount);
		}
	}
}