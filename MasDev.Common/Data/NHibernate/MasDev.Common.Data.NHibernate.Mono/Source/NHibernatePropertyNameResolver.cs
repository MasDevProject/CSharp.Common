using System;
using System.Linq.Expressions;
using MasDev.Data.NHibernate;


namespace MasDev.Data
{
	public class NHibernatePropertyNameResolver : IPropertyNameResolver
	{
		public string Resolve<TModel, TKey> (Expression<Func<TModel, TKey>> propertyExpr) where TModel : IModel
		{
			return NHibernateUtils.ExtractPropertyNameFromProxiedExpression (propertyExpr);
		}
	}
}

