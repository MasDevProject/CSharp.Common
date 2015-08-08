using MasDev.Common;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;

namespace MasDev.Services
{
	public class ModelAccessValidator<TModel, TDto> : EntityAccessValidator<TModel>
		where TModel : class, IEntity
		where TDto : class, IEntity
	{
		readonly IEntityAccessValidator<TDto> _dtoValidator;

		protected ModelAccessValidator ()
		{
			_dtoValidator = Injector.Resolve<IEntityAccessValidator<TDto>> ();
		}

		protected override void Validate (int id, ICallingContext context, AccessType accessType)
		{
			_dtoValidator.Validate (id, context, accessType);
		}
	}

	public class AsyncModelAccessValidator<TModel, TDto> : AsyncEntityAccessValidator<TModel>
		where TModel : class, IEntity
		where TDto : class, IEntity
	{
		readonly IEntityAccessValidator<TDto> _dtoValidator;

		protected AsyncModelAccessValidator ()
		{
			_dtoValidator = Injector.Resolve<IEntityAccessValidator<TDto>> ();
		}

		protected override async Task ValidateAsync (int id, ICallingContext context, AccessType accessType)
		{
			await _dtoValidator.EnsureCanAccessAsync (id, context, accessType);
		}
	}
}

