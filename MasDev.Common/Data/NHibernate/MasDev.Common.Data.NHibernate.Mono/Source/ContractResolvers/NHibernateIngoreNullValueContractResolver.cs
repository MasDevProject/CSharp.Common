using System;
using NHibernate.Proxy;
using Newtonsoft.Json.Serialization;

namespace MasDev.Newtonsoft.ContractResolvers
{
	public class NHibernateContractResolver : IgnoreEmptyContractResolver
	{
		protected override JsonContract CreateContract (Type objectType)
		{
			if (typeof(INHibernateProxy).IsAssignableFrom (objectType))
				return base.CreateContract (objectType.BaseType);
			else
				return base.CreateContract (objectType);
		}
	}
}

