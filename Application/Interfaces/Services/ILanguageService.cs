using Sindika.AspNet.app015.Application.DTOs.Language;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface ILanguageService : IBaseCrudService<LanguageDTO, LanguagePaginationDTO, CreateLanguageParam>
    {

    }
}
