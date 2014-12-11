using System;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;


namespace MasDev.Common.Newtonsoft.ContractResolvers
{
	public class IgnoreEmptyContractResolver : DefaultContractResolver
	{
		public IgnoreEmptyContractResolver (bool shareCache = false) : base (shareCache)
		{
		}


		protected override JsonProperty CreateProperty (MemberInfo member,
		                                                MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty (member, memberSerialization);
			bool isDefaultValueIgnored =
				((property.DefaultValueHandling ?? DefaultValueHandling.Ignore)
				& DefaultValueHandling.Ignore) != 0;

			if (isDefaultValueIgnored
			    && !typeof(string).IsAssignableFrom (property.PropertyType)
			    && typeof(IEnumerable).IsAssignableFrom (property.PropertyType)) {
				Predicate<object> shouldSerialize = obj => {
					var collection = property.ValueProvider.GetValue (obj) as ICollection;
					return collection == null || collection.Count != 0;
				};

				Predicate<object> oldShouldSerialize = property.ShouldSerialize;
				property.ShouldSerialize = oldShouldSerialize != null
					? o => oldShouldSerialize (o) && shouldSerialize (o)
					: shouldSerialize;
			}
			return property;
		}
	}
}

