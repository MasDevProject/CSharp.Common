using MasDev.Services.Modeling;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MasDev.Common;

namespace MasDev.Services
{
	public class BaseCrudService<TDto, TModel, TRepository> : BaseService, ICrudService<TDto>
		where TDto: class, IEntity
		where TModel : class, IModel, new()
		where TRepository : class, IAsyncRepository<TModel>
	{
		#region Properties

		protected TRepository Repository { get { return Injector.Resolve<TRepository> (); } }

		protected ICommunicationMapper<TDto, TModel> CommunicationMapper { get { return GetMapper<TDto, TModel> (); } }

		protected IConsistencyValidator<TDto> ConsistencyValidator { get { return GetConsistencyValidator<TDto> (); } }

		protected IEntityAccessValidator<TDto> DtoAccessValidator { get { return GetDtoAccessValidator<TDto> (); } }

		protected IEntityAccessValidator<TModel> ModelAccessValidator { get { return GetModelAccessValidator<TModel> (); } }

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

			var conversionTasks = models.Select (m => MapAsync<TDto, TModel> (m)).ToList ();
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
			return await MapAsync<TDto, TModel> (model);
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
			return await MapAsync<TDto, TModel> (model);
		}

		public virtual async Task DeleteAsync (int id)
		{
			await ValidateAccessAsync (id, AccessType.Delete);

			var model = await Repository.ReadAsync (id);
			if (model == null)
				throw new NotFoundException (id);
			
			await Repository.DeleteAsync (model);
		}

		protected virtual async Task<TModel> MapAsync (TDto dto)
		{
			return await MapAsync<TDto, TModel> (dto);
		}

		protected virtual async Task<TDto> MapAsync (TModel model)
		{
			return await MapAsync<TDto, TModel> (model);
		}

		protected virtual async Task ValidateAccessAsync (int id, AccessType accessType)
		{
			if (DtoAccessValidator != null)
				await DtoAccessValidator.EnsureCanAccessAsync (id, CallingContext, accessType);
			if (ModelAccessValidator != null)
				await ModelAccessValidator.EnsureCanAccessAsync (id, CallingContext, accessType);
		}
	}
}

