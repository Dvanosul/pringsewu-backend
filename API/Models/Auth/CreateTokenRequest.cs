
using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.API.Models.Auth
{
    public class CreateTokenRequest
    {

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;
    }
}
