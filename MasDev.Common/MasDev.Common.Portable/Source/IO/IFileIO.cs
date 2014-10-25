using System.IO;
using System.Threading.Tasks;


namespace MasDev.Common.IO
{
	public interface IFileIO
	{
		bool Exists (string path);



		void Delete (string path);



		void WriteAll (string text, string path);



		void WriteAll (byte[] bytes, string path);



		void WriteAll (Stream stream, string path);



		Task WriteAllAsync (string text, string path);



		Task WriteAllAsync (byte[] bytes, string path);



		Task WriteAllAsync (Stream stream, string path);



		string ReadString (string path);



		byte[] ReadBytes (string path);



		Stream ReadStream (string path);



		Task<string> ReadStringAsync (string path);



		Task<byte[]> ReadBytesAsync (string path);



		void CreateDirectory (string path);
	}
}

