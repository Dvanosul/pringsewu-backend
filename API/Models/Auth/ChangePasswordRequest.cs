using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.API.Models.Auth
{
    public class ChangePasswordRequest
    {
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("confirmationPassword")]
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}
