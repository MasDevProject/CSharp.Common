using MasDev.IO;

namespace MasDev.Common
{
	//TODO
	public class FileIO : IFileIO
	{
		public bool Exists (string path)
		{
			throw new System.NotImplementedException ();
		}

		public void Delete (string path)
		{
			throw new System.NotImplementedException ();
		}

		public void WriteAll (string text, string path)
		{
			throw new System.NotImplementedException ();
		}

		public void WriteAll (byte[] bytes, string path)
		{
			throw new System.NotImplementedException ();
		}

		public void WriteAll (System.IO.Stream stream, string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.Threading.Tasks.Task WriteAllAsync (string text, string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.Threading.Tasks.Task WriteAllAsync (byte[] bytes, string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.Threading.Tasks.Task WriteAllAsync (System.IO.Stream stream, string path)
		{
			throw new System.NotImplementedException ();
		}

		public string ReadString (string path)
		{
			throw new System.NotImplementedException ();
		}

		public byte[] ReadBytes (string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.IO.Stream ReadStream (string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.Threading.Tasks.Task<string> ReadStringAsync (string path)
		{
			throw new System.NotImplementedException ();
		}

		public System.Threading.Tasks.Task<byte[]> ReadBytesAsync (string path)
		{
			throw new System.NotImplementedException ();
		}

		public void CreateDirectory (string path)
		{
			throw new System.NotImplementedException ();
		}
	}
}

