namespace MasDev.Services.Test
{
	public static class UserEndpoints
	{
		const string Base = "api/user/";

		public const string Create = Base;
		public const string ReadMany = Base;
		public const string Read = Base + "{id}";
		public const string Update = Read;
		public const string Delete = Read;
	}
}

