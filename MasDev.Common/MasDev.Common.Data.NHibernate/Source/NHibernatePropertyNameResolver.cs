using MasDev.Common.Modeling;
using System;
using System.Linq.Expressions;
using MasDev.Common.Data.NHibernate;


namespace MasDev.Common.Data.NHibernate
{
	public class NHibernatePropertyNameResolver : IPropertyNameResolver
	{
		public string Resolve<TModel, TKey> (Expression<Func<TModel, TKey>> propertyExpr) where TModel : IModel
		{
			return NHibernateUtils.ExtractPropertyNameFromProxiedExpression (propertyExpr);
		}
	}
}

