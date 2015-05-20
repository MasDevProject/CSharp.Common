using MasDev.Common;


namespace MasDev.Services.Modeling
{
	public sealed class Identity : IIdentity
	{
		public int Id { get; set; }

		public int Roles { get; set; }

		public int Flag { get; set; }
	}
}

