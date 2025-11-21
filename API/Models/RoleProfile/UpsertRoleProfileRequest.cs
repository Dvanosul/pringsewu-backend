
using System.Text.Json.Serialization;
using Sindika.AspNet.Validation.Attributes.General;

namespace Sindika.AspNet.app015.API.Models.RoleProfile
{
    public class UpsertRoleProfileRequest
    {
        [Mandatory]
        [JsonPropertyName("roleId")]
        public Guid RoleId { get; set; }

        [Mandatory]
        [JsonPropertyName("changedProfiles")]
        public IEnumerable<Guid> ChangedProfiles { get; set; } = null!;
    }
}
