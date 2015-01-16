using MasDev.Common.Modeling;
using System.Linq.Expressions;
using System;

namespace MasDev.Common.Data.NHybernate
{
	public class NHibernateSortableQueryResult<T> : NHibernateQueryResult<T>, ISortableQueryResult<T> where T : IModel
	{
		public IOrderedEntitySet<T> OrderBy (Expression<Func<T, object>> predicate)
		{

		}
	}
}

