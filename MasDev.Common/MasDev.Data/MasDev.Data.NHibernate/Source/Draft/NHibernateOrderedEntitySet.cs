using MasDev.Common.Data.NHybernate;
using MasDev.Common.Modeling;
using NHibernate;
using System;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;

namespace MasDev.Common.Data.NHybernate
{
	public class NHibernateOrderedEntitySet<T> : NHibernateQueryResult<T>, IOrderedEntitySet<T> where T : IModel
	{
		IQueryOverOrderBuilder<T,T> _builder;

		public NHibernateOrderedEntitySet (IQueryOver<T, T> queryOver, Expression<Func<T, object>> predicate) : base (queryOver)
		{
			_builder = _queryOver2.OrderBy (predicate);
		}

		public IOrderedEntitySet<T> ThenBy (Expression<Func<T, object>> predicate)
		{
			_builder = _queryOver2.ThenBy (predicate);
			return this;
		}

		public IQueryResult<T> Desc
		{
			get { 
				_queryOver2 = _builder.Desc;
				return this;
			}
		}

		public IQueryResult<T> Asc
		{
			get {
				_queryOver2 = _builder.Asc;
				return this;
			}
		}
	}
}

