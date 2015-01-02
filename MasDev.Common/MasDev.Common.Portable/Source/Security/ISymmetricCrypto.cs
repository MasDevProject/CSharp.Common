
namespace MasDev.Security
{
	public interface ISymmetricCrypto
	{
		string Encrypt (string plainText, string password);

		string Decrypt (string cypherText, string password);
	}
}

