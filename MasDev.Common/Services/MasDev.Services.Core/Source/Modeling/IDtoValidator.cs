using MasDev.Services.Modeling;

namespace MasDev.Services.Modeling
{
	public interface IDtoValidator<TDto> where TDto : IDto
	{
		void Validate (TDto dto, Identity identity);
	}
}

