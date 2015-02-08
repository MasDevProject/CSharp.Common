

using System;
using System.Linq.Expressions;

namespace MasDev.Common.Utils
{
    public class DynamicWrapper<T>
    {
        readonly dynamic _wrapped;

        public DynamicWrapper(dynamic obj)
        {
            _wrapped = obj;
        }

        public T Value { get { return _wrapped; } }

        public dynamic DynamicValue { get { return _wrapped; } }

        public dynamic this[string property]
        {
            get { return _wrapped[property]; }
            set { _wrapped[property] = value; }
        }

        public DynamicWrapperProperty<TProperty> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var member = propertyExpression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyExpression));
            return Property<TProperty>(member.Member.Name);
        }

        public TProperty PropertyValue<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return Property(propertyExpression).Value;
        }

        public bool HasProperty<TProperty>(Expression<Func<T, TProperty>> property)
        {
            return Property(property).Exists;
        }



        public DynamicWrapperProperty<TProperty> Property<TProperty>(string propertyName)
        {
            try
            {
                var propertyValue = _wrapped[propertyName];
                return new DynamicWrapperProperty<TProperty>(propertyValue, true);
            }
            catch
            {
                return new DynamicWrapperProperty<TProperty>(null, false);
            }
        }

        public TProperty PropertyValue<TProperty>(string propertyName)
        {
            return Property<TProperty>(propertyName).Value;
        }

        public bool HasProperty<TProperty>(string propertyName)
        {
            return Property<TProperty>(propertyName).Exists;
        }

        public TProperty PropertyValueOrDefault<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var property = Property(propertyExpression);
            return property.Exists ? property.Value : default(TProperty);
        }


        public TProperty PropertyValueOrDefault<TProperty>(string propertyName)
        {
            var property = Property<TProperty>(propertyName);
            return property.Exists ? property.Value : default(TProperty);
        }
    }


    public class DynamicWrapperProperty<T>
    {
        public DynamicWrapper<T> DynamicValue
        {
            get
            {
                if (!Exists)
                    throw new ArgumentException("Dynamic value does not exist");
                return _value;
            }
        }

        public T Value
        {
            get
            {
                if (!Exists)
                    throw new ArgumentException("Dynamic value does not exist");
                return _value.Value;
            }
        }

        public bool Exists { get { return _value != null; } }

        readonly DynamicWrapper<T> _value;

        internal DynamicWrapperProperty(dynamic propertyValue, bool exists)
        {
            if (exists)
                _value = new DynamicWrapper<T>(propertyValue);
        }
    }
}
