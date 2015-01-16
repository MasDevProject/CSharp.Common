using NHibernate;
using System;
using MasDev.Common.Modeling;
using MasDev.Common.Data.NHybernate;
using System.Linq.Expressions;

namespace MasDev.Common.Data.NHibernate
{
	public class NHibernateEntitySet<T> : NHibernateQueryResult<T>, IEntitySet<T> where T : IModel
	{
		public NHibernateEntitySet (IQueryOver<T, T> queryOver) : base (queryOver)
		{
		}

		public IEntitySet<T> Where (Expression<Func<T, bool>> predicate)
		{
			ThrowIfNoQuery2 ();
			_queryOver2 = _queryOver2.Where (predicate);
			return this;
		}

		public IOrderedEntitySet<T> OrderBy (Expression<Func<T, object>> predicate)
		{
			return new NHibernateOrderedEntitySet<T> (_queryOver2, predicate);
		}

		public ISortableQueryResult<T> Take (uint count)
		{
			ThrowIfNoQuery2 ();
			_queryOver1 = _queryOver2.Take ((int)count);
			_queryOver2 = null;
			return this;
		}

		public ISortableQueryResult<T> Skip (uint count)
		{
			ThrowIfNoQuery2 ();
			_queryOver1 = _queryOver2.Skip ((int)count);
			_queryOver2 = null;
			return this;
		}
	}
}

