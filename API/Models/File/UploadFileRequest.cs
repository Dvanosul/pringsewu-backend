
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.File
{
    public class UploadFileRequest
    {
        [Mandatory]
        [JsonPropertyName("folder")]
        public string Folder { get; set; } = string.Empty;

        [Mandatory]
        [JsonPropertyName("file")]
        public required IFormFile File { get; set; }
    }
}
