using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using MasDev.Common.Rest;
using System.Linq;
using MasDev.Common.IO;


namespace MasDev.Common.Rest
{
	public class MultipartContent
	{
		public MultipartFormDataCollection Parameters { get; private set; }



		public List<MultipartFile> Files { get; private set; }



		public MultipartContent (MultipartFormDataCollection data, List<MultipartFile> files)
		{
			Parameters = data;
			Files = files;
		}



		public MultipartFile RequireUniqueFile ()
		{
			if (Files.Count > 1 || Files.Count == 0)
				throw new BadRequestException ("A single file is required", 1000);

			return Files.Single ();
		}
	}





	public class MultipartFormDataCollection
	{
		readonly Dictionary<string,string> _data;



		public MultipartFormDataCollection (Dictionary<string, string> data)
		{
			_data = data;
		}



		public string this [string key]
		{
			get { return _data [key]; }
		}



		public T Parse<T> (string key)
		{
			return JsonConvert.DeserializeObject<T> (this [key]);
		}
	}





	public class MultipartFile
	{
		public Stream Content{ get; private set; }



		public string Name { get; private set; }



		public string FileName { get; private set; }



		public string Mime { get; private set; }



		public MultipartFile (Stream content, string name, string fileName, string mime)
		{
			Content = content;
			Name = name;
			FileName = fileName;
			Mime = mime;
		}



		public MimedStream AsMimedStream ()
		{
			return new MimedStream (Content, Mime, FileName);
		}
	}
}

