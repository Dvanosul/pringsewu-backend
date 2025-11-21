using Sindika.AspNet.app015.Application.DTOs.Menu;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IMenuService : IBaseCrudService<MenuDetailDTO, MenuPaginationDTO, MenuParam>
    {
        Task MoveAsync(MoveMenuParam param, Guid id);
        Task<List<MenuDTO>> GetUserMenuListAsync(Guid userId, string roleActive);
        Task<List<MenuDTO>> GetAllMenuListAsync();
    }
}
