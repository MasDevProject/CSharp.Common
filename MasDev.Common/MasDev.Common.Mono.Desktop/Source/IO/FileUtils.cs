using System.Threading.Tasks;
using System.IO;
using System;
using System.Text;


namespace MasDev.Common.IO
{
	public static class FileUtils
	{
		const int _bufSize = 4096;



		public static async Task<string> ReadAsStringAsync (string filename)
		{
			using (TextReader file = File.OpenText (filename))
			{     
				var builder = new StringBuilder ();
				var line = string.Empty;
				while ((line = await file.ReadLineAsync ()) != null)
					builder.Append (line);

				return builder.ToString ();
			}
		}



		public static async Task WriteAsync (string filename, string text)
		{
			byte[] buff = StringUtils.GetBytes (text);
			using (var file = new FileStream (filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, buff.Length, true))
			{
				await file.WriteAsync (buff, 0, buff.Length);
			}
		}
	}
}

