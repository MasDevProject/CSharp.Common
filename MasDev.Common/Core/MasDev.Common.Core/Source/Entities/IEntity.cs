namespace MasDev.Common
{
	public interface IEntity
	{
		int Id { get; set; }
	}

	public interface IChildEntity : IEntity
	{
		IEntity Parent { get ; set; }
	}

	public interface IChildEntity<TParent> : IChildEntity where TParent : IEntity
	{
		new TParent Parent { get ; set; }
	}
}