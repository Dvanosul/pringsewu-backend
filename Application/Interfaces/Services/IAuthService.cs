
using Sindika.AspNet.app015.Application.DTOs.Auth;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IAuthService : IBaseService
    {
        Task<TokenDTO> GetToken(TokenParam param);
        Task<TokenDTO> LoginAsync(LoginParam param);
        Task ChangePasswordAsync(string email, string newPassword);
    }
}
