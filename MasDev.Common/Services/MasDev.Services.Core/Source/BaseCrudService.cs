using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MasDev.Common;
using MasDev.Services.Auth;

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
	}

	public class BaseCrudService<TDto, TModel, TRepository> : BaseService, ICrudService<TDto>
		where TDto: class, IEntity
		where TModel : class, IModel, new()
		where TRepository : class, IRepository<TModel>
	{
		#region Properties

		protected TRepository Repository { get { return Injector.Resolve<TRepository> (); } }

		protected ICommunicationMapper<TDto, TModel> CommunicationMapper { get { return Injector.Resolve <ICommunicationMapper<TDto, TModel>> (); } }

		protected IConsistencyValidator<TDto> ConsistencyValidator { get { return Injector.Resolve<IConsistencyValidator<TDto>> (); } }

		protected IEntityAccessValidator<TDto> DtoAccessValidator { get { return Injector.Resolve<IEntityAccessValidator<TDto>> (); } }

		protected IEntityAccessValidator<TModel> ModelAccessValidator { get { return Injector.Resolve<IEntityAccessValidator<TModel>> (); } }

		#endregion

		protected virtual IQueryable<TModel> Query (ICallingContext context)
		{
			return Repository.Query;
		}

		public virtual async Task<TDto> CreateAsync (TDto dto)
		{
			await ValidateConsistencyAsync (dto);
			await ValidateAccessAsync (dto, AccessType.Create);
			var model = await MapAsync (dto);
			await Repository.CreateAsync (model);
			return await MapAsync (model);
		}

		public virtual async Task<IList<TDto>> ReadPagedAsync (int skip, int take)
		{
			var models = await Query (CallingContext).Skip (skip).Take (take).ToListAsync ();

			if (!models.Any ())
				return Enumerable.Empty<TDto> ().ToList ();

			if (DtoAccessValidator != null) {
				var authorizedModels = new List<TModel> ();
				foreach (var model in models) {
					try {
						await ValidateAccessAsync (model, AccessType.Read);
					} catch {
						authorizedModels.Add (model);
					}
				}
				models = authorizedModels;
			}

			var conversionTasks = models.Select (MapAsync).ToList ();
			await Task.WhenAll (conversionTasks);

			return conversionTasks.Select (t => t.Result).ToList ();
		}

		public virtual async Task<TDto> ReadAsync (int id)
		{
			await ValidateAccessAsync (id, AccessType.Read);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new NotFoundException (id);

			await ValidateAccessAsync (model, AccessType.Read);
			return await MapAsync (model);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto)
		{
			await ValidateConsistencyAsync (dto);
			await ValidateAccessAsync (dto, AccessType.Update);

			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new NotFoundException (dto.Id);

			await ValidateAccessAsync (model, AccessType.Update);

			if (CommunicationMapper.IsAsync)
				await CommunicationMapper.MapForUpdateAsync (dto, model, CallingContext);
			else
				CommunicationMapper.MapForUpdate (dto, model, CallingContext);
			
			await Repository.UpdateAsync (model);
			return await MapAsync (model);
		}

		public virtual async Task DeleteAsync (int id)
		{
			await ValidateAccessAsync (id, AccessType.Delete);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new NotFoundException (id);
			
			await Repository.DeleteAsync (model);
		}

		#region Protected methods

		protected virtual async Task<TDto> MapAsync (TModel model)
		{
			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (model, CallingContext) : 
				CommunicationMapper.Map (model, CallingContext);
		}

		protected virtual async Task<TModel> MapAsync (TDto dto)
		{
			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, CallingContext);
			else
				ConsistencyValidator.Validate (dto, CallingContext);

			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (dto, CallingContext) : 
				CommunicationMapper.Map (dto, CallingContext);
		}

		protected virtual async Task ValidateConsistencyAsync (TDto dto)
		{
			if (ConsistencyValidator == null)
				return;

			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, CallingContext);
			else
				ConsistencyValidator.Validate (dto, CallingContext);
		}

		protected virtual async Task ValidateAccessAsync (TDto dto, AccessType accessType)
		{
			if (DtoAccessValidator == null)
				return;

			await DtoAccessValidator.EnsureCanAccessAsync (dto, CallingContext, accessType);
		}

		protected virtual async Task ValidateAccessAsync (TModel model, AccessType accessType)
		{
			if (ModelAccessValidator == null)
				return;

			await ModelAccessValidator.EnsureCanAccessAsync (model, CallingContext, accessType);
		}

		protected virtual async Task ValidateAccessAsync (int id, AccessType accessType)
		{
			if (DtoAccessValidator != null)
				await DtoAccessValidator.EnsureCanAccessAsync (id, CallingContext, accessType);
			if (ModelAccessValidator != null)
				await ModelAccessValidator.EnsureCanAccessAsync (id, CallingContext, accessType);
		}

		#endregion
	}
}

