using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Services
{
    public abstract class BaseUserUserTypeService<T, TContext, TPaginationDTO, TEntity, TPEntity> : BaseAppService<T, TContext>, IBaseUserUserTypeService<TPaginationDTO>, IBaseService where T : BaseService where TContext : DbContext where TPaginationDTO : RolePaginationDTO where TEntity : UserUserType where TPEntity : class
    {
        protected readonly IBaseUserUserTypeRepository<TEntity, TPEntity> _repository;
        protected readonly IRoleRepository _roleRepository;

        public BaseUserUserTypeService(IConfiguration configuration, ILogger<T> logger, IUnitOfWork<TContext> unitOfWork, IBaseUserUserTypeRepository<TEntity, TPEntity> repository, IRoleRepository roleRepository)
            : base(configuration, logger, unitOfWork)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        public async Task<PaginationResponse<TPaginationDTO>> GetRolePaginationAsync(PaginationQuery paginationQuery, Guid? identifier = null)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _roleRepository.GetPaginationAsync(paginationQuery, (q) => q.Where(r => r.UserType!.Code == _repository.GetDiscriminator()));
                var isEnabledDict = identifier != null ? await _repository.GetEnabledRoles(identifier.Value, itemCountResponse.Items.Select(r => r.Id)) : null;

                var items = itemCountResponse.Items.Select(role =>
                {
                    var item = role.Adapt<TPaginationDTO>();
                    item.IsEnabled = isEnabledDict?.Contains(role.Id) ?? false;

                    return item;
                }).Adapt<List<TPaginationDTO>>();

                PaginationResponse<TPaginationDTO> result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }
        public async Task<PaginationResponse<TPaginationDTO>> GetEnabledRolePaginationAsync(PaginationQuery paginationQuery, Guid identifier)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery, (q) => q.Where((c) => c.Role != null)
                                                                                                     .Where(_repository.IdentifierFilter(identifier))
                                                                                                     .Include(c => c.Role));
                List<TPaginationDTO> items = itemCountResponse.Items.Select(i =>
                {
                    var item = i.Role.Adapt<TPaginationDTO>();
                    item.IsEnabled = i.IsEnabled;

                    return item;
                }).Adapt<List<TPaginationDTO>>();

                PaginationResponse<TPaginationDTO> result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        protected string GeneratePaginationKey(PaginationQuery paginationQuery)
        {
            if (paginationQuery == null)
            {
                throw new ArgumentNullException("paginationQuery");
            }

            string text = string.Join(";", paginationQuery.Filters.Select((FilterOption f) => f.Field + "," + f.Value));
            string text2 = string.Join(";", paginationQuery.Sorts.Select((SortOption s) => s.Field + "," + s.Direction));
            string value = paginationQuery.Pagination.Page.ToString();
            string value2 = paginationQuery.Pagination.PageSize.ToString();
            string text3 = $"{GenerateKey()}:Pagination:{value}:{value2}";
            if (paginationQuery.Filters.Any())
            {
                text3 = text3 + ":" + text;
            }

            if (paginationQuery.Sorts.Any())
            {
                text3 = text3 + ":" + text2;
            }

            return text3;
        }

        protected string GenerateKey(string? obj = null)
        {
            string text = typeof(T).Name.Replace("Service", string.Empty);
            if (!string.IsNullOrEmpty(obj))
            {
                return text + ":" + obj;
            }

            return text;
        }
    }
}
