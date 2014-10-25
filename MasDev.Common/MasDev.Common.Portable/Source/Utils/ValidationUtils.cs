using System.Text.RegularExpressions;


namespace MasDev.Common
{
	public static class ValidationUtils
	{
		public static bool IsEmailValid(string email)
		{
			return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match (email).Success;
		}
	}
}

