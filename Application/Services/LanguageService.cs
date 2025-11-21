using Sindika.AspNet.app015.Application.DTOs.Language;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Entities;

namespace Sindika.AspNet.app015.Application.Services
{
    public class LanguageService : BaseCrudService<
        LanguageService,
        Context,
        LanguageDTO,
        LanguagePaginationDTO,
        CreateLanguageParam,
        Language,
        ILanguageRepository>, ILanguageService
    {
        public LanguageService(
            IConfiguration configuration,
            ILogger<LanguageService> logger,
            IUnitOfWork<Context>
            unitOfWork, ILanguageRepository repository
            ) : base(configuration, logger, unitOfWork, repository)
        {
        }

    }
}
