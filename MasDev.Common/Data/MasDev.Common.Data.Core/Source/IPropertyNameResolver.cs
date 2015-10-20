using System;
using System.Linq.Expressions;

namespace MasDev.Data
{
    public interface IPropertyNameResolver
    {
        string Resolve<TModel, TKey>(Expression<Func<TModel, TKey>> propertyExpr) where TModel : IModel;
    }
}
