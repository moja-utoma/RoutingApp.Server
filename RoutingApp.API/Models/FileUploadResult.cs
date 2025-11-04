namespace RoutingApp.API.Models
{
	public class FileUploadResult
	{
		public string BlobName { get; set; }
		public long Size { get; set; }
		public string ContentType { get; set; }
		public string Version { get; set; }
	}
}
