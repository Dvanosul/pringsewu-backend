using Sindika.AspNet.app015.Application.DTOs.Profile;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IProfileService : IBaseCrudService<ProfileDTO, ProfilePaginationDTO, ProfileParam>
    {
        Task<Guid> DuplicateAsync(Guid param);
    }
}
