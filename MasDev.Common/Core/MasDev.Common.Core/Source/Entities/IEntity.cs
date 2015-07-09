namespace MasDev.Common
{
	public interface IEntity
	{
		int Id { get; set; }
	}

	public interface IChildEntity<TParent> : IEntity where TParent : IEntity
	{
		TParent Parent { get ; set; }
	}
}