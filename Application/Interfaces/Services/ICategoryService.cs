using Sindika.AspNet.app015.Application.DTOs.Category;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface ICategoryService : IBaseCrudService<CategoryDTO, CategoryPaginationDTO, CategoryParam>
    {

    }
}
