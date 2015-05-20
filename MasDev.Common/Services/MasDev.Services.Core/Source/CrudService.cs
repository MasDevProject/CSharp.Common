using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MasDev.Common;

namespace MasDev.Services
{
	public class CrudService<TDto, TModel, TRepository> : ICrudService<TDto>
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

		protected virtual IQueryable<TModel> Query (IIdentityContext context)
		{
			return Repository.Query;
		}

		public virtual async Task<TDto> CreateAsync (TDto dto, IIdentityContext context)
		{
			await ValidateConsistencyAsync (dto, context);
			await ValidateAccessAsync (dto, context, AccessType.Create);
			var model = await MapAsync (dto, context);
			await Repository.CreateAsync (model);
			return await MapAsync (model, context);
		}

		public virtual async Task<IList<TDto>> ReadAsync (int skip, int take, IIdentityContext context)
		{
			var models = await Query (context).Skip (skip).Take (take).ToListAsync ();

			if (!models.Any ())
				return Enumerable.Empty<TDto> ().ToList ();

			if (DtoAccessValidator != null) {
				var authorizedModels = new List<TModel> ();
				foreach (var model in models) {
					try {
						await ValidateAccessAsync (model, context, AccessType.Read);
					} catch {
						authorizedModels.Add (model);
					}
				}
				models = authorizedModels;
			}

			var conversionTasks = models.Select (m => MapAsync (m, context)).ToList ();
			await Task.WhenAll (conversionTasks);

			return conversionTasks.Select (t => t.Result).ToList ();
		}

		public virtual async Task<TDto> ReadAsync (int id, IIdentityContext context)
		{
			await ValidateAccessAsync (id, context, AccessType.Read);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new NotFoundException (id);

			await ValidateAccessAsync (model, context, AccessType.Read);
			return await MapAsync (model, context);
		}

		public virtual async Task<TDto> UpdateAsync (TDto dto, IIdentityContext context)
		{
			await ValidateConsistencyAsync (dto, context);
			await ValidateAccessAsync (dto, context, AccessType.Update);

			var model = await Repository.ReadAsync (dto.Id);
			if (model == null)
				throw new NotFoundException (dto.Id);

			await ValidateAccessAsync (model, context, AccessType.Update);

			if (CommunicationMapper.IsAsync)
				await CommunicationMapper.MapForUpdateAsync (dto, model, context);
			else
				CommunicationMapper.MapForUpdate (dto, model, context);
			
			await Repository.UpdateAsync (model);
			return await MapAsync (model, context);
		}

		public virtual async Task DeleteAsync (int id, IIdentityContext context)
		{
			await ValidateAccessAsync (id, context, AccessType.Delete);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new NotFoundException (id);
			
			await Repository.DeleteAsync (model);
		}

		#region Protected methods

		protected virtual async Task<TDto> MapAsync (TModel model, IIdentityContext context)
		{
			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (model, context) : 
				CommunicationMapper.Map (model, context);
		}

		protected virtual async Task<TModel> MapAsync (TDto dto, IIdentityContext context)
		{
			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, context);
			else
				ConsistencyValidator.Validate (dto, context);

			return CommunicationMapper.IsAsync ? 
				await CommunicationMapper.MapAsync (dto, context) : 
				CommunicationMapper.Map (dto, context);
		}

		protected virtual async Task ValidateConsistencyAsync (TDto dto, IIdentityContext context)
		{
			if (ConsistencyValidator == null)
				return;

			if (ConsistencyValidator.IsAsync)
				await ConsistencyValidator.ValidateAsync (dto, context);
			else
				ConsistencyValidator.Validate (dto, context);
		}

		protected virtual async Task ValidateAccessAsync (TDto dto, IIdentityContext context, AccessType accessType)
		{
			if (DtoAccessValidator == null)
				return;

			DtoAccessValidator.EnsureCanAccessAsync (dto, context, accessType);
		}

		protected virtual async Task ValidateAccessAsync (TModel model, IIdentityContext context, AccessType accessType)
		{
			if (ModelAccessValidator == null)
				return;

			await ModelAccessValidator.EnsureCanAccessAsync (model, context, accessType);
		}

		protected virtual async Task ValidateAccessAsync (int id, IIdentityContext context, AccessType accessType)
		{
			if (DtoAccessValidator != null)
				await DtoAccessValidator.EnsureCanAccessAsync (id, context, accessType);
			if (ModelAccessValidator != null)
				await ModelAccessValidator.EnsureCanAccessAsync (id, context, accessType);
		}

		#endregion
	}
}

