
namespace MasDev.Security
{
	public interface IHashedPassword
	{
		byte[] PasswordHash { get; set; }

		byte[] PasswordSalt { get; set; }
	}
}

