using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MasDev.Extensions;

namespace MasDev.IO
{
	public sealed class CloudBlobFileIO : IFileIO
	{
		readonly CloudStorageAccount _account;
		readonly CloudBlobClient _client;
		readonly CloudBlobContainer _container;

		public CloudBlobFileIO (CloudStorageAccount account, string container)
		{
			_account = account;
			_client = account.CreateCloudBlobClient ();
			_container = _client.GetContainerReference (container);
			_container.CreateIfNotExists ();
		}

		public bool Exists (string path)
		{
			return GetBlob (path).Exists ();
		}

		public void Delete (string path)
		{
			GetBlob (path).DeleteIfExists ();
		}

		public void WriteAll (string text, string path)
		{
			WriteAll (Encoding.UTF8.GetBytes (text), path);
		}

		public void WriteAll (byte[] bytes, string path)
		{
			var blob = GetBlob (path);
			blob.UploadFromByteArray (bytes, 0, bytes.Length);
		}

		public void WriteAll (Stream stream, string path)
		{
			var blob = GetBlob (path);
			blob.UploadFromStream (stream);
		}

		public async Task WriteAllAsync (string text, string path)
		{
			await WriteAllAsync (text.AsByteArray (), path);
		}

		public async Task WriteAllAsync (byte[] bytes, string path)
		{
			var blob = GetBlob (path);
			await blob.UploadFromByteArrayAsync (bytes, 0, bytes.Length);
		}

		public async Task WriteAllAsync (Stream stream, string path)
		{
			var blob = GetBlob (path);
			await blob.UploadFromStreamAsync (stream);
		}

		public string ReadString (string path)
		{
			return GetBlob (path).DownloadText ();
		}

		public byte[] ReadBytes (string path)
		{
			var blob = GetBlob (path);
			using (var memStream = new MemoryStream ())
			using (var sourceStream = blob.OpenRead ()) {
				sourceStream.CopyTo (memStream);
				return memStream.ToArray ();
			}
		}

		public Stream ReadStream (string path)
		{
			return GetBlob (path).OpenRead ();
		}

		public async Task<string> ReadStringAsync (string path)
		{
			return await GetBlob (path).DownloadTextAsync ();
		}

		public async Task<byte[]> ReadBytesAsync (string path)
		{
			var blob = GetBlob (path);
			using (var memStream = new MemoryStream ())
			using (var sourceStream = await blob.OpenReadAsync ()) {
				await sourceStream.CopyToAsync (memStream);
				return memStream.ToArray ();
			}
		}

		public void CreateDirectory (string path)
		{
			// HEHE :)
		}

		CloudBlockBlob GetBlob (string path)
		{
			var normalizedPath = path.Replace ('/', '_').Replace ('\\', '_');
			return _container.GetBlockBlobReference (normalizedPath);
		}
	}
}
