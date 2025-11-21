using System.Text.Json.Serialization;

namespace Sindika.AspNet.app015.Application.DTOs.Auth;

public class LoginParam
{
    /// <summary>
    /// Username or email.
    /// </summary>
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
