using Azure.Storage.Blobs;
using RoutingApp.API.Models;

namespace RoutingApp.API.Services
{
	public interface IFileStorageService
	{
		Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder = null);
		Task<(Stream stream, string contentType, string fileName)> DownloadFileAsync(string blobName);
	}

	public class AzureBlobStorageService : IFileStorageService
	{
		private readonly IConfiguration _configuration;
		private readonly BlobContainerClient _containerClient;

		public AzureBlobStorageService(IConfiguration configuration)
		{
			_configuration = configuration;
			var connectionString = _configuration["BlobStorage:ConnectionString"];
			var containerName = _configuration["BlobStorage:ContainerName"];
			_containerClient = new BlobContainerClient(connectionString, containerName);
		}

		public async Task<(Stream stream, string contentType, string fileName)> DownloadFileAsync(string blobName)
		{
			if (string.IsNullOrWhiteSpace(blobName))
				throw new ArgumentException("Blob name is required");

			var blobClient = _containerClient.GetBlobClient(blobName);

			if (!await blobClient.ExistsAsync())
				throw new FileNotFoundException($"Blob '{blobName}' not found");

			var properties = await blobClient.GetPropertiesAsync();
			var stream = new MemoryStream();
			await blobClient.DownloadToAsync(stream);
			stream.Position = 0;

			var contentType = properties.Value.ContentType ?? "application/octet-stream";
			var fileName = Path.GetFileName(blobName);

			return (stream, contentType, fileName);
		}


		public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder = null)
		{
			if (file == null || file.Length == 0)
				throw new Exception("File is empty");

			await _containerClient.CreateIfNotExistsAsync();

			var blobName = string.IsNullOrWhiteSpace(folder)
				? $"{Guid.NewGuid()}_{file.FileName}"
				: $"{folder}/{Guid.NewGuid()}_{file.FileName}";

			var blobClient = _containerClient.GetBlobClient(blobName);

			using var stream = file.OpenReadStream();
			var response = await blobClient.UploadAsync(stream, overwrite: true);

			return new FileUploadResult
			{
				BlobName = blobName,
				Size = file.Length,
				ContentType = file.ContentType ?? "application/octet-stream",
				Version = response.Value.VersionId
			};
		}
	}

}
