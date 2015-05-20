using MasDev.Common;

namespace MasDev.Data
{
	public interface IModel : IEntity
	{
	}

	public interface IChildModel :IModel, IChildEntity
	{
	}

	public interface IChildModel<TParentModel> :IModel, IChildEntity<TParentModel> where TParentModel : IModel
	{
	}
}

