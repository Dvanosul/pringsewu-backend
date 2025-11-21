using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.Gender;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;

namespace Sindika.AspNet.app015.Application.Services
{
    public class GenderService : BaseCrudService<
        GenderService,
        Context,
        GenderDTO,
        GenderPaginationDTO,
        GenderParam,
        Gender,
        IGenderRepository>, IGenderService
    {
        public GenderService(
            IConfiguration configuration,
            ILogger<GenderService> logger,
            IUnitOfWork<Context>
            unitOfWork, IGenderRepository repository
            ) : base(configuration, logger, unitOfWork, repository)
        {
        }

    }
}
