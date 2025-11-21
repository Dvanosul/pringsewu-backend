using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.API.Models.Menu
{
    public class MoveMenuRequest
    {
        [JsonPropertyName("parentMenu")]
        public Guid ParentMenu { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
