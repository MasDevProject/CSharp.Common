using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MasDev.Services
{
	public interface ICrudService<TDto>  where TDto : IDto
	{
		Task<TDto> CreateAsync (TDto dto, IIdentityContext context);

		Task<IList<TDto>> ReadAsync (int skip, int take, IIdentityContext context);

		Task<TDto> ReadAsync (int id, IIdentityContext context);

		Task<TDto> UpdateAsync (TDto dto, IIdentityContext context);

		Task DeleteAsync (int id, IIdentityContext context);
	}
}

