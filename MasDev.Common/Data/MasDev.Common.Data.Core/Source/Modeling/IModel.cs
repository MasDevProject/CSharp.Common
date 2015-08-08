using MasDev.Common;

namespace MasDev.Data
{
	public interface IModel : IEntity
	{
	}

	public interface IChildModel<TParentModel> :IModel, IChildEntity<TParentModel> where TParentModel : IModel
	{
	}
}

