using MasDev.Data;
using AutoMapper;

namespace MasDev.Services.Modeling
{
	public abstract class DtoMapper<TDto, TModel> : IDtoValidator<TDto>
		where TDto : IDto
		where TModel : IModel
	{
		public TModel Map (TDto dto, Identity identity)
		{
			(this as IDtoValidator<TDto>).Validate (dto, identity);
			return MapInternal (dto, identity);
		}

		void IDtoValidator<TDto>.Validate (TDto dto, Identity identity)
		{
			(this as DtoMapper<TDto, TModel>).Validate (dto, identity);
		}

		protected abstract void Validate (TDto dto, Identity identity);

		protected abstract TModel MapInternal (TDto dto, Identity identity);

		public abstract TDto Map (TModel model, Identity identity, int? scope = null);

		public virtual void MapForUpdate (TDto dto, TModel model, Identity identity, int? scope)
		{
			Mapper.DynamicMap (dto, model);
		}
	}
}

