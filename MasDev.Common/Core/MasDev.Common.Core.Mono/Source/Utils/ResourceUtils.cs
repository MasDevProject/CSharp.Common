using System;
using System.Reflection;
using System.IO;

namespace MasDev.Mono.Utils
{
	public static class ResourceUtils
	{
		public static void SaveResource (string resourceName, string defaultNamespace, string targetFile)
		{
			var completeResourceName = string.Format ("{0}.{1}", defaultNamespace, resourceName);

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(completeResourceName))
			{
				if (s == null)
					throw new Exception("The specified resource could not be loaded. [" + completeResourceName + "]");

				var buffer = new byte[s.Length];
				s.Read(buffer, 0, buffer.Length);

				using (var writer = new BinaryWriter(File.Open(targetFile, FileMode.Create)))
					writer.Write(buffer);
			}
		}
	}
}

