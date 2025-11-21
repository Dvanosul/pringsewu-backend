
using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Services
{
    public class UserInfoService : BaseAppService<UserInfoService, Context>, IUserInfoService
    {
        private readonly IUserUserTypeRepository _userUserTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleProfileRepository _roleProfileRepository;

        public UserInfoService(
            IConfiguration configuration,
            ILogger<UserInfoService> logger,
            IUnitOfWork<Context> unitOfWork,
            IUserUserTypeRepository userUserTypeRepository,
            IUserRepository userRepository,
            IRoleProfileRepository roleProfileRepository
            ) : base(configuration, logger, unitOfWork)
        {
            _userUserTypeRepository = userUserTypeRepository;
            _userRepository = userRepository;
            _roleProfileRepository = roleProfileRepository;
        }
        public async Task<PaginationResponse<UserRolePaginationDTO>> GetUserRolesPaginationAsync(Guid userId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _userUserTypeRepository.GetUserRolesPaginationAsync(userId, paginationQuery);
                var rolesPageAccessDict = await _roleProfileRepository.GetRolesPageAccessPreviewAsync(itemCountResponse.Items.Select(c => c.RoleId!.Value));

                var items = itemCountResponse.Items.Select(r =>
                {
                    var item = r.Role.Adapt<UserRolePaginationDTO>();
                    item.PageAccess = rolesPageAccessDict.GetValueOrDefault(r.RoleId!.Value, []);

                    return item;
                }).ToList();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }


        public async Task<PaginationResponse<PagePaginationDTO>> GetRolePageAccessPaginationAsync(Guid roleId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _roleProfileRepository.GetRolePageAccessPaginationAsync(roleId, paginationQuery);
                var items = itemCountResponse.Items.Adapt<List<PagePaginationDTO>>();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<PaginationResponse<EventPaginationDTO>> GetRolePageEventAccessPaginationAsync(Guid roleId, Guid pageId, PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");
                var itemCountResponse = await _roleProfileRepository.GetRolePageEventAccessPaginationAsync(roleId, pageId, paginationQuery);
                var items = itemCountResponse.Items.Adapt<List<EventPaginationDTO>>();

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select((c) => c.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<UserInfoDTO> GetUserInfo(Guid userId)
        {
            try
            {
                StartOperation("GET");

                var user = await _userRepository.GetAsync(userId) ?? throw new NotFoundException("User not found.");
                var roles = await _userUserTypeRepository.GetUserRolesAsync(userId);

                var result = user.Adapt<UserInfoDTO>();
                result.Roles = roles.Adapt<List<RoleDTO>>();

                AppendRecords(userId.ToString());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }
    }
}
