using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingApp.Data.Entities
{
	public class FileRecord
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string BlobName { get; set; }
		public string ContentType { get; set; }
		public long Size { get; set; }
		public string Version { get; set; }
		public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
	}
}
