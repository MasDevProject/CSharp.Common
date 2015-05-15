using System;
using MasDev.Services.Modeling;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MasDev.Services
{
	public interface ICrudService<TDto>  where TDto : IDto
	{
		Task<TDto> CreateAsync (TDto dto);

		Task<IList<TDto>> ReadAsync (int skip, int take);

		Task<TDto> ReadAsync (int id);

		Task<TDto> UpdateAsync (TDto dto);

		Task DeleteAsync (int id);
	}
}

