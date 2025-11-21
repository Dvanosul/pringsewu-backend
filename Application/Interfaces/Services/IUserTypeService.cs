using Sindika.AspNet.app015.Application.DTOs.UserType;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IUserTypeService : IBaseCrudService<UserTypeDTO, UserTypePaginationDTO, CreateUserTypeParam>
    {

    }
}
