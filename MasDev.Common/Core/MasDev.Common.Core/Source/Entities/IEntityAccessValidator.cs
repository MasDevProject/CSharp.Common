using System.Threading.Tasks;
using System;

namespace MasDev.Common
{
	public enum AccessType
	{
		Create,
		Read,
		Update,
		Delete
	}

	public interface IEntityAccessValidator<TEntity> 
		where TEntity : IEntity
	{
		void Validate (TEntity entity, IIdentityContext context, AccessType accessType);

		void Validate (int id, IIdentityContext context, AccessType accessType);

		Task ValidateAsync (TEntity entity, IIdentityContext context, AccessType accessType);

		Task ValidateAsync (int id, IIdentityContext context, AccessType accessType);

		bool IsAsync { get; }
	}

	public abstract class EntityAccessValidator<TEntity> : IEntityAccessValidator<TEntity> 
		where TEntity : class, IEntity
	{
		protected abstract void Validate (int id, IIdentityContext context, AccessType accessType);

		public virtual void Validate (TEntity entity, IIdentityContext context, AccessType accessType)
		{
			Validate (entity.Id, context, accessType);
		}

		void IEntityAccessValidator<TEntity>.Validate (int id, IIdentityContext context, AccessType accessType)
		{
			(this as EntityAccessValidator<TEntity>).Validate (id, context, accessType);
		}

		public Task ValidateAsync (TEntity entity, IIdentityContext context, AccessType accessType)
		{
			throw new NotSupportedException ();
		}

		public Task ValidateAsync (int id, IIdentityContext context, AccessType accessType)
		{
			throw new NotSupportedException ();
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncEntityAccessValidator<TEntity> : IEntityAccessValidator<TEntity> 
		where TEntity : class, IEntity
	{
		protected abstract Task ValidateAsync (int id, IIdentityContext context, AccessType accessType);

		public virtual void Validate (TEntity entity, IIdentityContext context, AccessType accessType)
		{
			throw new NotSupportedException ();
		}

		public void Validate (int id, IIdentityContext context, AccessType accessType)
		{
			throw new NotSupportedException ();
		}

		public virtual async Task ValidateAsync (TEntity entity, IIdentityContext context, AccessType accessType)
		{
			await ValidateAsync (entity.Id, context, accessType);
		}

		async Task IEntityAccessValidator<TEntity>. ValidateAsync (int id, IIdentityContext context, AccessType accessType)
		{
			await (this as AsyncEntityAccessValidator<TEntity>).ValidateAsync (id, context, accessType);
		}

		public bool IsAsync { get { return true; } }
	}





	public static class EntityAccessExtensions
	{
		public static async Task EnsureCanAccessAsync<TEntity> (this IEntityAccessValidator<TEntity> validator, TEntity entity, IIdentityContext context, AccessType accessType) where TEntity : IEntity
		{
			if (validator.IsAsync)
				await validator.ValidateAsync (entity, context, accessType);
			else
				validator.Validate (entity, context, accessType);
		}

		public static async Task EnsureCanAccessAsync<TEntity> (this IEntityAccessValidator<TEntity> validator, int id, IIdentityContext context, AccessType accessType) where TEntity : IEntity
		{
			if (validator.IsAsync)
				await validator.ValidateAsync (id, context, accessType);
			else
				validator.Validate (id, context, accessType);
		}
	}
}