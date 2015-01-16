using System.IO;


namespace MasDev.IO
{
	public class MimedStream
	{
		public Stream Stream { get; private set; }


		public string Mime { get; private set; }


		public string FileName { get; private set; }



		public MimedStream (Stream stream, string mime, string fileName = null)
		{
			Stream = stream;
			Mime = mime;
			FileName = fileName;
		}
	}
}

