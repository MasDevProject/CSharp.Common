
namespace MasDev.Data
{
	public interface IChildModel<TParentModel> :IModel where TParentModel : IModel
	{
		TParentModel Parent { get; set; }
	}
}

