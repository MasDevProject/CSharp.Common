using System.Threading.Tasks;
using System;
using MasDev.Common;

namespace MasDev.Common
{
	public interface IConsistencyValidator<TEntity> where TEntity : IEntity
	{
		void Validate (TEntity dto, IIdentityContext context);

		Task ValidateAsync (TEntity dto, IIdentityContext context);

		bool IsAsync { get; }
	}

	public abstract class ConsistencyValidator<TEntity> : IConsistencyValidator<TEntity> where TEntity : class, IEntity
	{
		protected abstract void Validate (TEntity dto, IIdentityContext context);

		void IConsistencyValidator<TEntity>.Validate (TEntity dto, IIdentityContext context)
		{
			if (dto == null)
				throw new InputException ();

			(this as ConsistencyValidator<TEntity>).Validate (dto, context);
		}

		public Task ValidateAsync (TEntity dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Asyncronous validation not supported");
		}

		public bool IsAsync { get { return false; } }
	}

	public abstract class AsyncConsistencyValidator<TEntity> : IConsistencyValidator<TEntity> where TEntity : class, IEntity
	{
		protected abstract Task ValidateAsync (TEntity dto, IIdentityContext context);

		public void Validate (TEntity dto, IIdentityContext context)
		{
			throw new NotSupportedException ("Syncronous validation not supported");
		}

		async Task IConsistencyValidator<TEntity>.ValidateAsync (TEntity dto, IIdentityContext context)
		{
			if (dto == null)
				throw new InputException ();
			
			await (this as AsyncConsistencyValidator<TEntity>).ValidateAsync (dto, context);
		}

		public bool IsAsync { get { return true; } }
	}
}

