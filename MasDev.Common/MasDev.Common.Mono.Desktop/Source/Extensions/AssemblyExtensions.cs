using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;
using MasDev.Common.Utils;


namespace MasDev.Common.Extensions
{
	public static class AssemblyExtensions
	{
		public static IEnumerable<Assembly> LoadRefrencedAssemblies (this Assembly assembly)
		{
			return assembly.GetReferencedAssemblies ()
				.Select (a =>
			{
				try
				{
					return Assembly.Load (a);
				} catch (Exception)
				{
					return null;
				}
			}).Where (Check.IsNotNull);
		}



		public static Type[] TryGetExportedTypes (this Assembly assembly)
		{
			try
			{
				return assembly.GetExportedTypes ();
			} catch (Exception)
			{
				return null;
			}
		}



		public static bool ContainsNamespace (this Assembly assembly, string ns)
		{
			var assemblyTypes = assembly.GetExportedTypes ();
			return assemblyTypes.Any (type => type.Namespace == ns);
		}



		public static bool ContainsNamespace (this Assembly assembly, string ns, bool failOnError)
		{
			try
			{
				return assembly.ContainsNamespace (ns);
			} catch (Exception)
			{
				if (failOnError)
					throw;
				return false;
			}
		}
	}
}

