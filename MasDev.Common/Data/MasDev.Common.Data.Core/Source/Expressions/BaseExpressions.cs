using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace MasDev.Data.Expressions
{
    class IdEqualsExpressionBuilder<TModel> : AtomicExpressionBuilder<TModel, int> where TModel : IModel
    {
        static readonly string _idPropertyName = typeof(IModel).GetRuntimeProperties().Single().Name;
        readonly MemberExpression _memberAccess;

        public IdEqualsExpressionBuilder(ParameterExpression parameterExpression)
            : base(parameterExpression)
        {
            _memberAccess = Expression.MakeMemberAccess(parameterExpression, typeof(TModel).GetRuntimeProperty(_idPropertyName));
        }

        public override Expression BuildAtomicExpression(int constantValue)
        {
            return Expression.Equal(_memberAccess, Expression.Constant(constantValue));
        }
    }

    public sealed class PropertyEqualsExpressionBuilderFactory<TModel, TProperty> where TModel : IModel
    {
        readonly PropertyInfo _propertyInfo;

        public PropertyEqualsExpressionBuilderFactory(Expression<Func<TModel, TProperty>> propertyExpression, IPropertyNameResolver nameResolver)
        {
            var propertyName = nameResolver.Resolve(propertyExpression);
            _propertyInfo = typeof(TModel).GetRuntimeProperties().Single(p => p.Name == propertyName);
        }

        public AtomicExpressionBuilder<TModel, TProperty> Build(ParameterExpression parameterExpression)
        {
            return new PropertyEqualsExpressionBuilder<TModel, TProperty>(parameterExpression, _propertyInfo);
        }

        class PropertyEqualsExpressionBuilder<TIModel, TIProperty> : AtomicExpressionBuilder<TIModel, TIProperty> where TIModel : IModel
        {
            readonly PropertyInfo _property;

            public PropertyEqualsExpressionBuilder(ParameterExpression expr, PropertyInfo prop)
                : base(expr)
            {
                _property = prop;
            }

            public override Expression BuildAtomicExpression(TIProperty constantValue)
            {
                var memberAccess = Expression.MakeMemberAccess(ParameterExpression, _property);
                return Expression.Equal(memberAccess, Expression.Constant(constantValue));
            }
        }
    }
}

