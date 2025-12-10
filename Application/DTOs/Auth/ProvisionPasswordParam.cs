using System;

namespace Sindika.AspNet.app015.Application.DTOs.Auth;

public class ProvisionPasswordParam
{
    public Guid UserId { get; set; }
    public string Password { get; set; } = string.Empty;
}
