using MasDev.Services.Modeling;
using MasDev.Patterns.Injection;
using System;
using MasDev.Data;
using MasDev.Services.Auth;
using MasDev.Common;
using System.IO;

namespace MasDev.Services
{
	public static class App
	{
		public static string ConfigFolder { get; private set; }

		public static object PerRequestLifestyle { get; private set; }

		static Func<IDisposable> _executionContextFacotry;

		public static IDisposable NewExecutionContextScope ()
		{
			return _executionContextFacotry ();
		}

		public static void Configure (string configFolder, object perRequestLifeStyle, Func<IDisposable> executionContextFactory)
		{
			ConfigFolder = configFolder;
			PerRequestLifestyle = perRequestLifeStyle;
			_executionContextFacotry = executionContextFactory;
		}

		public static string ConfigFile (string relativePath)
		{
			if (string.IsNullOrEmpty (ConfigFolder))
				throw new NotSupportedException ("Must set configFolderFirst");
			
			return Path.Combine (ConfigFolder, relativePath);
		}

		static void EnsureIsSetup ()
		{
			if (PerRequestLifestyle == null)
				throw new NotSupportedException ("Must setup first");
		}

		public static void RegisterAuthManager (this IDependencyContainer container, Func<IAuthorizationManager> authManagerFactory)
		{
			EnsureIsSetup ();
			container.AddDependency<IAuthorizationManager> (authManagerFactory, LifeStyles.Singleton);
		}

		public static void RegisterCrudService<TDto, TServiceInterface, TServiceImplementation> (this IDependencyContainer container)
			where TDto : IEntity
			where TServiceInterface : class, ICrudService<TDto>
			where TServiceImplementation : class, TServiceInterface, new()
		{
			EnsureIsSetup ();
			container.AddDependency<TServiceInterface, TServiceImplementation> (LifeStyles.Singleton);
		}

		public static void RegisterUnitOfWork<TUnitOfWork> (this IDependencyContainer container)
			where TUnitOfWork : class, IUnitOfWork, new()
		{
			EnsureIsSetup ();
			container.AddDependency<IUnitOfWork, TUnitOfWork> (PerRequestLifestyle);
		}

		public static void RegisterAccessTokenStore<TAccessTokenStore> (this IDependencyContainer container)
			where TAccessTokenStore : class, ICredentialsRepository, new()
		{
			EnsureIsSetup ();
			container.AddDependency<ICredentialsRepository, TAccessTokenStore> (PerRequestLifestyle);
		}

		public static void RegisterRepository<TModel, TRepositoryInterface, TRepositoryImplementation> (this IDependencyContainer container)
			where TModel : class, IModel, new()
			where TRepositoryInterface : class, IRepository<TModel>
			where TRepositoryImplementation : class, TRepositoryInterface, new()
		{
			EnsureIsSetup ();
			container.AddDependency<TRepositoryInterface, TRepositoryImplementation> (PerRequestLifestyle);
		}

		public static void RegisterCommunicationMapper<TDto, TModel, TCommunicationMapper> (this IDependencyContainer container)
			where TDto : class, IEntity
			where TModel : class, IModel, new()
			where TCommunicationMapper : class, ICommunicationMapper<TDto, TModel>, new()
		{
			EnsureIsSetup ();
			container.AddDependency<ICommunicationMapper<TDto, TModel>, TCommunicationMapper> (LifeStyles.Singleton);
		}

		public static void RegisterConsistencyValidator<TDto, TConsistencyValidator> (this IDependencyContainer container)
			where TDto : class, IEntity
			where TConsistencyValidator : class, IConsistencyValidator<TDto>, new()
		{
			EnsureIsSetup ();
			container.AddDependency<IConsistencyValidator<TDto>, TConsistencyValidator> (LifeStyles.Singleton);
		}

		public static void RegisterAccessValidator<TEntity, TDataAccessValidator> (this IDependencyContainer container)
			where TEntity : class, IEntity
			where TDataAccessValidator : class, IEntityAccessValidator<TEntity>, new()
		{
			EnsureIsSetup ();
			container.AddDependency<IEntityAccessValidator<TEntity>, TDataAccessValidator> (LifeStyles.Singleton);
		}
	}
}

