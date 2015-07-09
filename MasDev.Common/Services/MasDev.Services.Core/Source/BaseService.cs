using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Common;
using System.Threading.Tasks;
using MasDev.Data;
using MasDev.Services.Modeling;

namespace MasDev.Services
{
	public class BaseService : IService
	{
		public IAuthorizationManager AuthorizationManager { get { return Injector.Resolve<IAuthorizationManager> (); } }

		public async Task AuthorizeAsync (int? minimumRoles = null)
		{
			await AuthorizationManager.AuthorizeAsync (minimumRoles);
		}

		public ICallingContext CallingContext { get { return Injector.Resolve<ICallingContext> (); } }


		public IIdentity CurrentIdentity {
			get {
				var context = CallingContext;
				return context == null ? null : context.Identity;
			}
		}

		protected void ThrowIfNotFound (object obj)
		{
			if (obj == null)
				throw new NotFoundException ();
		}

		protected ICommunicationMapper<TDto, TModel> GetMapper<TDto, TModel> ()
			where TDto: class, IEntity
			where TModel : class, IModel, new()
		{ 
			return Injector.Resolve <ICommunicationMapper<TDto, TModel>> (); 
		}

		protected IConsistencyValidator<TDto> GetConsistencyValidator<TDto> ()
			where TDto: class, IEntity
		{ 
			return Injector.Resolve<IConsistencyValidator<TDto>> ();
		}

		protected IEntityAccessValidator<TDto> GetDtoAccessValidator<TDto> ()
			where TDto: class, IEntity
		{ 
			return Injector.Resolve<IEntityAccessValidator<TDto>> (); 
		}

		protected IEntityAccessValidator<TModel> GetModelAccessValidator<TModel> () 
			where TModel : class, IModel, new()
		{ 
			return Injector.Resolve<IEntityAccessValidator<TModel>> ();  
		}

		protected virtual async Task<TDto> MapAsync<TDto, TModel> (TModel model)
			where TDto: class, IEntity
			where TModel : class, IModel, new()
		{
			var mapper = GetMapper<TDto, TModel> ();
			return mapper.IsAsync ? 
				await mapper.MapAsync (model, CallingContext) : 
				mapper.Map (model, CallingContext);
		}

		protected virtual async Task<TModel> MapAsync<TDto, TModel> (TDto dto)
			where TDto: class, IEntity
			where TModel : class, IModel, new()
		{
			await ValidateConsistencyAsync (dto);

			var communicationMapper = GetMapper<TDto, TModel> ();
			return communicationMapper.IsAsync ?
				await communicationMapper.MapAsync (dto, CallingContext) :
				communicationMapper.Map (dto, CallingContext);
		}

		protected virtual async Task ValidateConsistencyAsync<TDto> (TDto dto)
			where TDto: class, IEntity
		{
			var consistencyValidator = GetConsistencyValidator<TDto> ();
			if (consistencyValidator == null)
				return;

			if (consistencyValidator.IsAsync)
				await consistencyValidator.ValidateAsync (dto, CallingContext);
			else
				consistencyValidator.Validate (dto, CallingContext);
		}

		protected virtual async Task ValidateDtoAccessAsync <TDto> (TDto dto, AccessType accessType)
			where TDto: class, IEntity
		{
			var accessValidator = GetDtoAccessValidator<TDto> ();
			if (accessValidator == null)
				return;

			await accessValidator.EnsureCanAccessAsync (dto, CallingContext, accessType);
		}

		protected virtual async Task ValidateModelAccessAsync<TModel> (TModel model, AccessType accessType)
			where TModel : class, IModel, new()
		{
			var accessValidator = GetModelAccessValidator<TModel> ();
			if (accessValidator == null)
				return;

			await accessValidator.EnsureCanAccessAsync (model, CallingContext, accessType);
		}
	}
}

