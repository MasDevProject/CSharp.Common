﻿using MasDev.Services.Modeling;
using MasDev.Patterns.Injection;
using System;
using MasDev.Data;
using MasDev.Services.Auth;
using MasDev.Common;

namespace MasDev.Services
{
	public static class AppInjector
	{
		public static object PerRequestLifestyle { get; set; }

		static void EnsurePerRequestLifestyleIsSet ()
		{
			if (PerRequestLifestyle == null)
				throw new NotSupportedException ("Must set per request lifestyle firts");
		}

		public static void RegisterCrudService<TDto, TServiceInterface, TServiceImplementation> (this IDependencyContainer container)
			where TDto : IEntity
			where TServiceInterface : class, ICrudService<TDto>
			where TServiceImplementation : class, TServiceInterface, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<TServiceInterface, TServiceImplementation> (LifeStyles.Singleton);
		}

		public static void RegisterUnitOfWork<TUnitOfWork> (this IDependencyContainer container)
			where TUnitOfWork : class, IUnitOfWork, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<IUnitOfWork, TUnitOfWork> (PerRequestLifestyle);
		}

		public static void RegisterAccessTokenStore<TAccessTokenStore> (this IDependencyContainer container)
			where TAccessTokenStore : class, IAccessTokenStore, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<IAccessTokenStore, TAccessTokenStore> (PerRequestLifestyle);
		}

		public static void RegisterRepository<TModel, TRepositoryInterface, TRepositoryImplementation> (this IDependencyContainer container)
			where TModel : class, IModel, new()
			where TRepositoryInterface : class, IRepository<TModel>
			where TRepositoryImplementation : class, TRepositoryInterface, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<TRepositoryInterface, TRepositoryImplementation> (PerRequestLifestyle);
		}

		public static void RegisterCommunicationMapper<TDto, TModel, TCommunicationMapper> (this IDependencyContainer container)
			where TDto : class, IEntity
			where TModel : class, IModel, new()
			where TCommunicationMapper : class, ICommunicationMapper<TDto, TModel>, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<ICommunicationMapper<TDto, TModel>, TCommunicationMapper> (LifeStyles.Singleton);
		}

		public static void RegisterConsistencyValidator<TDto, TConsistencyValidator> (this IDependencyContainer container)
			where TDto : class, IEntity
			where TConsistencyValidator : class, IConsistencyValidator<TDto>, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<IConsistencyValidator<TDto>, TConsistencyValidator> (LifeStyles.Singleton);
		}

		public static void RegisterAccessValidator<TEntity, TDataAccessValidator> (this IDependencyContainer container)
			where TEntity : class, IEntity
			where TDataAccessValidator : class, IEntityAccessValidator<TEntity>, new()
		{
			EnsurePerRequestLifestyleIsSet ();
			container.AddDependency<IEntityAccessValidator<TEntity>, TDataAccessValidator> (LifeStyles.Singleton);
		}
	}
}

