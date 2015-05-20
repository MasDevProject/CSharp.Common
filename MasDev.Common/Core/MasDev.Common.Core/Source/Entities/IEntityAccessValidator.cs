using System.Threading.Tasks;
using System;

namespace MasDev.Common
{
	public interface IEntityAccessValidator<TEntity> 
		where TEntity : IEntity
	{
		void Validate (TEntity entity, IIdentityContext context);

		void Validate (int id, IIdentityContext context);

		Task ValidateAsync (TEntity entity, IIdentityContext context);

		Task ValidateAsync (int id, IIdentityContext context);

		bool IsAsync { get; }
	}

	public abstract class EntityAccessValidator<TEntity> : IEntityAccessValidator<TEntity> 
		where TEntity : class, IEntity
	{
		protected abstract void Validate (int id, IIdentityContext context);

		public virtual void Validate (TEntity entity, IIdentityContext context)
		{
			Validate (entity.Id, context);
		}

		void IEntityAccessValidator<TEntity>.Validate (int id, IIdentityContext context)
		{
			(this as EntityAccessValidator<TEntity>).Validate (id, context);
		}

		public Task ValidateAsync (TEntity entity, IIdentityContext context)
		{
			throw new NotSupportedException ();
		}

		public Task ValidateAsync (int id, IIdentityContext context)
		{
			throw new NotSupportedException ();
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncEntityAccessValidator<TEntity> : IEntityAccessValidator<TEntity> 
		where TEntity : class, IEntity
	{
		protected abstract Task ValidateAsync (int id, IIdentityContext context);

		public virtual void Validate (TEntity entity, IIdentityContext context)
		{
			throw new NotSupportedException ();
		}

		public void Validate (int id, IIdentityContext context)
		{
			throw new NotSupportedException ();
		}

		public virtual async Task ValidateAsync (TEntity entity, IIdentityContext context)
		{
			await ValidateAsync (entity.Id, context);
		}

		async Task IEntityAccessValidator<TEntity>. ValidateAsync (int id, IIdentityContext context)
		{
			await (this as AsyncEntityAccessValidator<TEntity>).ValidateAsync (id, context);
		}

		public bool IsAsync { get { return true; } }
	}





	public static class EntityAccessExtensions
	{
		public static async Task EnsureCanAccessAsync<TEntity> (this IEntityAccessValidator<TEntity> validator, TEntity entity, IIdentityContext context) where TEntity : IEntity
		{
			if (validator.IsAsync)
				await validator.ValidateAsync (entity, context);
			else
				validator.Validate (entity, context);
		}

		public static async Task EnsureCanAccessAsync<TEntity> (this IEntityAccessValidator<TEntity> validator, int id, IIdentityContext context) where TEntity : IEntity
		{
			if (validator.IsAsync)
				await validator.ValidateAsync (id, context);
			else
				validator.Validate (id, context);
		}
	}
}