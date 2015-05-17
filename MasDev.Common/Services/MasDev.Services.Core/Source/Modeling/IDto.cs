
namespace MasDev.Services.Modeling
{
	public interface IDto
	{
		int Id { get; set; }
	}

	public interface IChildDto<TDto> : IDto where TDto : IDto
	{
		TDto Parent { get; set; }
	}
}

