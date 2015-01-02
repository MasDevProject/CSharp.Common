﻿using System;
using MasDev.Droid.Utils;

namespace MasDev.Droid.ExtensionMethods
{
	public static class JavaObjectExtensions
	{
		public static TObject ToNetObject<TObject>(this Java.Lang.Object value)
		{
			if (value == null)
				return default(TObject);

			if (!(value is JavaHolder))
				throw new InvalidOperationException("Unable to convert to .NET object. Only Java.Lang.Object created with .ToJavaObject() can be converted.");

			TObject returnVal;
			try { returnVal = (TObject) ((JavaHolder) value).Instance; }
			finally { value.Dispose(); }
			return returnVal;
		}

		public static Java.Lang.Object ToJavaObject<TObject>(this TObject value)
		{
			if (Equals(value, default(TObject)) && !typeof(TObject).IsValueType)
				return null;

			var holder = new JavaHolder(value);

			return holder;
		}
	}
}

