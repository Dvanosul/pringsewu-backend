using Sindika.AspNet.app015.Application.DTOs.Gender;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IGenderService : IBaseCrudService<GenderDTO, GenderPaginationDTO, GenderParam>
    {

    }
}
