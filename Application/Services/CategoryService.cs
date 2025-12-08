using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.Category;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services
{
    public class CategoryService : BaseCrudService<
        CategoryService,
        Context,
        CategoryDTO,
        CategoryPaginationDTO,
        CategoryParam,
        Category,
        ICategoryRepository>, ICategoryService
    {
        public CategoryService(
            IConfiguration configuration,
            ILogger<CategoryService> logger,
            IUnitOfWork<Context> unitOfWork,
            ICategoryRepository repository
        ) : base(configuration, logger, unitOfWork, repository)
        {
        }
    }
}
