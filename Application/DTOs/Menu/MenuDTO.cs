using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Menu
{
    public class MenuDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string InternalCode { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public bool IsExpanded { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PageName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EventName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuDTO>? Children { get; set; }
    }
}
