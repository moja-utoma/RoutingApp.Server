using Azure.Storage.Blobs;

namespace RoutingApp.API.Services
{
	public interface IFileStorageService
	{
		Task<string> UploadFileAsync(IFormFile file, string folder = null);
	}

	public class AzureBlobStorageService : IFileStorageService
	{
		private readonly IConfiguration _configuration;
		private readonly BlobContainerClient _containerClient;

		public AzureBlobStorageService(IConfiguration configuration)
		{
			_configuration = configuration;
			var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
			var containerName = _configuration["AzureBlobStorage:ContainerName"];
			_containerClient = new BlobContainerClient(connectionString, containerName);
		}

		public async Task<string> UploadFileAsync(IFormFile file, string folder = null)
		{
			if (file == null || file.Length == 0)
				throw new Exception("File is empty");

			var allowedExtensions = new[] { ".csv" };
			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

			if (!allowedExtensions.Contains(extension))
				throw new Exception("Not an acceptable file format, please upload a .csv file");

			await _containerClient.CreateIfNotExistsAsync();

			var blobName = string.IsNullOrWhiteSpace(folder)
				? $"{Guid.NewGuid()}_{file.FileName}"
				: $"{folder}/{Guid.NewGuid()}_{file.FileName}";

			var blobClient = _containerClient.GetBlobClient(blobName);

			using var stream = file.OpenReadStream();
			await blobClient.UploadAsync(stream, overwrite: true);

			return blobClient.Uri.ToString();
		}
	}

}
