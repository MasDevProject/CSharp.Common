using System.IO;
using System.Threading.Tasks;
using MasDev.Extensions;
using System.Text;
using System.Collections.Generic;
using MasDev.Utils;


namespace MasDev.IO
{
	public class FileIO : IFileIO
	{
		public bool Exists (string path)
		{
			return File.Exists (path);
		}



		public void Delete (string path)
		{
			File.Delete (path);
		}



		public void WriteAll (string text, string path)
		{
			File.WriteAllText (path, text);
		}



		public void WriteAll (byte[] bytes, string path)
		{
			File.WriteAllBytes (path, bytes);
		}



		public void WriteAll (Stream stream, string path)
		{
			using (var filestream = new FileStream (path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
				stream.CopyTo (filestream);
		}



		public string ReadString (string path)
		{
			return File.ReadAllText (path);
		}



		public byte[] ReadBytes (string path)
		{
			return File.ReadAllBytes (path);
		}



		public Stream ReadStream (string path)
		{
			return new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}



		public async Task WriteAllAsync (string text, string path)
		{
			using (var stream = new FileStream (path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				var bytes = text.AsByteArray ();
				await stream.WriteAsync (bytes, 0, bytes.Length);
			}
		}



		public async Task WriteAllAsync (byte[] bytes, string path)
		{
			using (var stream = new FileStream (path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				await stream.WriteAsync (bytes, 0, bytes.Length);
			}
		}



		public async Task WriteAllAsync (Stream stream, string path)
		{
			using (var s = new FileStream (path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
				await stream.CopyToAsync (s);
		}



		public async Task<string> ReadStringAsync (string path)
		{
			using (var stream = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				var builder = new StringBuilder ();
				var buffer = new byte[0x1000];
				int numRead;
				while ((numRead = await stream.ReadAsync (buffer, 0, buffer.Length)) != 0) {
					string text = StringUtils.GetString (buffer.CopyUntil<byte> (numRead));
					builder.Append (text);
				}
				return builder.ToString ();
			}
		}



		public async Task<byte[]> ReadBytesAsync (string path)
		{
			using (var stream = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				var buffer = new byte[0x1000];
				int numRead;
				var ret = new List<byte> ();
				while ((numRead = await stream.ReadAsync (buffer, 0, buffer.Length)) != 0) {
					ret.AddRange (buffer.CopyUntil<byte> (numRead));
				}
				return ret.ToArray ();
			}
		}



		public void CreateDirectory (string path)
		{
			Directory.CreateDirectory (path);
		}
	}





	public static class StaticFileIO
	{
		static readonly IFileIO _io = new FileIO ();



		public static bool Exists (string path)
		{
			return _io.Exists (path);
		}



		public static void Delete (string path)
		{
			_io.Delete (path);
		}



		public static void WriteAll (string text, string path)
		{
			_io.WriteAll (text, path);
		}



		public static void WriteAll (byte[] bytes, string path)
		{
			_io.WriteAll (bytes, path);
		}



		public static void WriteAll (Stream stream, string path)
		{
			_io.WriteAll (stream, path);
		}



		public static string ReadString (string path)
		{
			return _io.ReadString (path);
		}



		public static byte[] ReadBytes (string path)
		{
			return _io.ReadBytes (path);
		}



		public static Stream ReadStream (string path)
		{
			return _io.ReadStream (path);
		}



		public static async Task WriteAllAsync (string text, string path)
		{
			await _io.WriteAllAsync (text, path);
		}



		public static async Task WriteAllAsync (byte[] bytes, string path)
		{
			await _io.WriteAllAsync (bytes, path);
		}



		public static async Task WriteAllAsync (Stream stream, string path)
		{
			await _io.WriteAllAsync (stream, path);
		}



		public static async Task<string> ReadStringAsync (string path)
		{
			return await _io.ReadStringAsync (path);
		}



		public static async Task<byte[]> ReadBytesAsync (string path)
		{
			return await _io.ReadBytesAsync (path);
		}
	}
}

