using Sindika.AspNet.app015.Application.DTOs.UserType;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;

namespace Sindika.AspNet.app015.Application.Services
{
    public class UserTypeService : BaseCrudService<
        UserTypeService,
        Context,
        UserTypeDTO,
        UserTypePaginationDTO,
        CreateUserTypeParam,
        UserType,
        IUserTypeRepository>, IUserTypeService
    {
        public UserTypeService(
            IConfiguration configuration,
            ILogger<UserTypeService> logger,
            IUnitOfWork<Context>
            unitOfWork, IUserTypeRepository repository
            ) : base(configuration, logger, unitOfWork, repository)
        {
        }

    }
}
