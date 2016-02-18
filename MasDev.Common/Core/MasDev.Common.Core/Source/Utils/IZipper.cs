
namespace MasDev.Common
{
	public interface IZipper 
	{
		void Unzip (string zipFilePath, bool ingoreHiddenFilesAndDirectories = true);
		void Zip (string directoryPath, string outputPath, int compressionLevel = 6);
	}
}

