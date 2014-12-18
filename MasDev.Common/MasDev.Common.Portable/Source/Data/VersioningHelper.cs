using System;
using System.Collections.Generic;
using AutoMapper;
using MasDev.Common.Data;
using System.Linq.Expressions;
using System.Linq;
using MasDev.Common.Utils;


namespace MasDev.Common.Modeling
{
	public static class VersioningHelper
	{
		public static T RequireParent<T> (IModelVersioning<T> model) where T : class, IVersionedModel
		{
			var parent = model.Parent;
			if (parent == null)
				throw new ArgumentException ("This versioning of " + typeof(T).Name + "does not have a parent");
			return parent;
		}



		public static IEnumerable<T> RequireParents<T> (IEnumerable<IModelVersioning<T>> models) where T : class, IVersionedModel
		{
			return models == null ? null : RequireParentsInt (models);
		}



		static IEnumerable<T> RequireParentsInt<T> (IEnumerable<IModelVersioning<T>> models) where T : class, IVersionedModel
		{
			foreach (var model in models)
				yield return RequireParent (model);
		}



		public static T RequireCurrentVersion<T> (IVersionedModel<T> model) where T : class, IModelVersioning
		{
			var currentVersion = model.CurrentVersion;
			if (currentVersion == null)
				throw new ArgumentException ("This versioning of " + typeof(T).Name + "does not have a parent");
			return currentVersion;
		}



		public static IQueryable<TModelVersioning> RequireCurrentVersions<TVersionedModel, TModelVersioning> (IModelQueryFactory factory, IEnumerable<TVersionedModel> models)
			where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
			where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
		{
			var ids = models.Select (m => m.CurrentVersion.Id).ToArray ();
			var lambda = LinqHelper.BuildOrIdExpression<TModelVersioning> (ids);
			return factory.UnfilteredQueryForModel<TModelVersioning> ().Where (lambda);
		}






		static IEnumerable<T> RequireCurrentVersionsInt<T> (IEnumerable<IVersionedModel<T>> models) where T : class, IModelVersioning
		{
			foreach (var model in models)
				yield return RequireCurrentVersion (model);
		}



		public static TModelVersioning CreateObviousMapping<TModelVersioning> (IVersionedModel<TModelVersioning> model)
			where TModelVersioning : IModelVersioning
		{
			var version = Mapper.DynamicMap<TModelVersioning> (model);
			version.Id = 0;
			version.CreationUTC = DateTime.UtcNow;
			return version;
		}



		public static  IQueryable<TModelVersioning> RequireCurrentVersions<TVersionedModel, TModelVersioning> (IRepository<TVersionedModel> repository, Expression<Func<TVersionedModel, bool>> selector) 
			where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
			where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
		{
			var modelQuery = repository.Query.Where (selector);
			var versionQuery = repository.UnfilteredQueryForModel<TModelVersioning> ();

			return (
			    from model in modelQuery
			    join version in versionQuery on model.CurrentVersion.Id equals version.Id
			    select version
			);
		}


		public static TKey VersionedAssign<TModel, TKey> (TModel source, TModel destination, Func<TModel, TKey> property, ref bool shouldDoVersioning)
		{
			return VersionedAssign (source, destination, property, (a, b) => a.Equals (b), ref shouldDoVersioning);
		}


		public static TKey VersionedAssign<TModel, TKey> (TModel source, TModel destination, Func<TModel, TKey> property, Func<TKey,TKey, bool> comparer, ref bool shouldDoVersioning)
		{
			var sourcePropertyValue = property (source);
			var destinationPropertyValue = property (source);

			if (Check.BothNull (sourcePropertyValue, destinationPropertyValue))
				return sourcePropertyValue;

			if (!Check.BothNotNull (sourcePropertyValue, destinationPropertyValue)) {
				shouldDoVersioning = true;
				return sourcePropertyValue;
			}

			shouldDoVersioning = shouldDoVersioning || comparer (sourcePropertyValue, destinationPropertyValue);
			return sourcePropertyValue;
		}
	}
}

