
using System.Security.Claims;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.DTOs.Auth;
using Sindika.AspNet.app015.Application.DTOs.Event;
using Sindika.AspNet.app015.Application.DTOs.Page;
using Sindika.AspNet.app015.Application.DTOs.Role;
using Sindika.AspNet.app015.Application.DTOs.User;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Exceptions.Unauthorized;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.Application.Services
{
    public class AuthService : BaseAppService<AuthService, Context>, IAuthService
    {
        private readonly ISSOService _sSOService;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserUserTypeRepository _userUserTypeRepository;
        private readonly IDeveloperUserTypeRepository _developerUserTypeRepository;
        private readonly IEmployeeUserTypeRepository _employeeUserTypeRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRoleProfileRepository _roleProfileRepository;
        private readonly ISynchronizeCache _synchronizeCache;

        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IUnitOfWork<Context> unitOfWork,
            ISSOService sSOService,
            IJwtService jwtService,
            IRoleRepository roleRepository,
            IUserUserTypeRepository userUserTypeRepository,
            IDeveloperUserTypeRepository developerUserTypeRepository,
            IDeveloperRepository developerRepository,
            IEmployeeUserTypeRepository employeeUserTypeRepository,
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository,
            IProfileRepository profileRepository,
            IRoleProfileRepository roleProfileRepository,
            ISynchronizeCache synchronizeCache
            ) : base(configuration, logger, unitOfWork)
        {
            _sSOService = sSOService;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
            _userUserTypeRepository = userUserTypeRepository;
            _developerUserTypeRepository = developerUserTypeRepository;
            _developerRepository = developerRepository;
            _employeeUserTypeRepository = employeeUserTypeRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _roleProfileRepository = roleProfileRepository;
            _synchronizeCache = synchronizeCache;
        }

        public async Task<TokenDTO> GetToken(TokenParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("GET|UPSERT");
                var clientId = _configuration.GetValue<string>("SSOAuth:Keycloak:ClientId") ?? throw new ArgumentNullException($"ClientId is missing or empty in the configuration.");
                var keycloakTokenDTO = await _sSOService.ExchangeCodeForAccessTokenAsync<KeycloakTokenDTO>(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", param.Code),
                }, param.Platform);
                if (keycloakTokenDTO.Error != null && keycloakTokenDTO.ErrorDescription != null)
                {
                    throw new NotFoundException($"{keycloakTokenDTO.Error} : {keycloakTokenDTO.ErrorDescription}");
                }

                var keycloakUserDTO = _jwtService.GetUserInfoAsync<KeycloakUserDTO>(keycloakTokenDTO.AccessToken ?? "") ?? throw new NotFoundException("Keycloak User not found.");
                var keycloakRoles = keycloakUserDTO.ResourceAccess.Where(c => c.Key == clientId);
                var roles = new List<string>();
                if (keycloakRoles.Any())
                {
                    roles = keycloakRoles.FirstOrDefault().Value.Roles;
                }
                else
                {
                    throw new NotFoundException("Keycloak Roles not found.");
                }

                if (roles.Count == 0) throw new NotFoundException("Roles not found!");
                var userRoles = await _roleRepository.GetsByCodeAsync(roles);
                var userRoleDict = userRoles.GroupBy(c => c.Code).ToDictionary(c => c.Key, c => c.FirstOrDefault());

                var (user, validRoles) = await SynchronizeUser(keycloakUserDTO.Email, keycloakUserDTO.Name, keycloakUserDTO.Username, userRoles);
                if (validRoles.Count == 0) throw new NotFoundException("Valid Roles not found!");

                var roleCodeActive = validRoles.FirstOrDefault("");
                var roleActive = userRoleDict.GetValueOrDefault(roleCodeActive);

                var claims = new List<Claim>
                {
                    new Claim("name", keycloakUserDTO.Name),
                    new Claim("email", keycloakUserDTO.Email),
                    new Claim("family_name", keycloakUserDTO.LastName),
                    new Claim("given_name", keycloakUserDTO.FirstName),
                    new Claim("preferred_username", keycloakUserDTO.Username),
                    new Claim("user_id", user?.Id.ToString() ?? ""),
                    new Claim("user_type_id", roleActive?.UserTypeId.ToString() ?? ""),
                    new Claim("role_id", roleActive?.Id.ToString() ?? ""),
                    new Claim("is_super", user?.IsSuper.ToString() ?? "False"),
                };

                foreach (var role in validRoles)
                {
                    claims.Add(new Claim("roles", role));
                }

                var token = _jwtService.GenerateToken(claims, DateTime.UtcNow.AddDays(1));
                await _unitOfWork.CommitAsync();
                return new TokenDTO
                {
                    AccessToken = token,
                    RoleActive = roleCodeActive,
                    Roles = validRoles,
                    IdToken = keycloakTokenDTO?.IdToken ?? ""
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<TokenDTO> LoginAsync(LoginParam param)
        {
            try
            {
                StartOperation("GET");

                User? user = null;
                // Search by email first, if not found, search by username.
                user = await _userRepository.GetByIdentifiersAsync(
                    new Dictionary<string, object>
                    {
                        { "email", param.Username },
                    }
                );
                if (user is null)
                {
                    user = await _userRepository.GetByIdentifiersAsync(
                        new Dictionary<string, object>
                        {
                            { "username", param.Username },
                        }
                    ) ?? throw new UnauthorizedAccessAttemptException();
                }

                var ok = BCrypt.Net.BCrypt.Verify(param.Password, user.Password);
                if (!ok)
                    throw new UnauthorizedAccessAttemptException();

                // Load roles that are enabled for this user
                var userUserTypes = await _userUserTypeRepository.GetsAsync(q =>
                    q.Include("Role")
                    .Where(uut => uut.UserId == user.Id && uut.IsEnabled && uut.RoleId.HasValue)
                ) ?? new List<UserUserType>();

                var roleCodes = userUserTypes
                    .Where(uut => uut.Role != null)
                    .Select(uut => uut.Role!.Code)
                    .Distinct()
                    .ToList();

                if (roleCodes.Count == 0)
                    throw new NotFoundException("Roles not found!");

                // Resolve Role entities for claims (user_type_id, role_id, etc.)
                var roleEntities = await _roleRepository.GetsByCodeAsync(roleCodes);
                var roleDict = roleEntities
                    .GroupBy(r => r.Code)
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault());

                var roleActiveCode = roleCodes.First(); // choose the first enabled role as active
                var roleActive = roleDict.GetValueOrDefault(roleActiveCode);

                if (roleActive is null)
                    throw new NotFoundException("Active role not found.");

                var claims = new List<Claim>
                {
                    new Claim("name", user.Name ?? string.Empty),
                    new Claim("email", user.Email ?? string.Empty),
                    new Claim("family_name", string.Empty),
                    new Claim("given_name", string.Empty),
                    new Claim("preferred_username", user.Username ?? string.Empty),
                    new Claim("user_id", user.Id.ToString()),
                    new Claim("user_type_id", roleActive.UserTypeId.ToString()),
                    new Claim("role_id", roleActive.Id.ToString()),
                    new Claim("is_super", user.IsSuper.ToString())
                };

                foreach (var code in roleCodes)
                    claims.Add(new Claim("roles", code));

                var jwt = _jwtService.GenerateToken(claims, DateTime.UtcNow.AddDays(1));

                return new TokenDTO
                {
                    AccessToken = jwt,
                    RoleActive = roleActiveCode,
                    Roles = roleCodes,
                    IdToken = string.Empty
                };
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task ChangePasswordAsync(string email, string newPassword)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var user = await _userRepository.GetByIdentifiersAsync(
                    new Dictionary<string, object>
                    {
                        { "email", email },
                    }
                ) ?? throw new NotFoundException("User not found.");

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12);

                await _userRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }
        
        private async Task<List<string>> SynchronizeProfile(List<string> roles)
        {
            var userRoles = await _roleRepository.GetsByCodeAsync(roles);
            var profiles = await _profileRepository.GetsAsync(query => query.Where(c => roles.Contains(c.Code)));
            var profileDict = profiles.GroupBy(c => c.Code).ToDictionary(c => c.Key, c => c.FirstOrDefault());

            var existingRoleProfiles = await _roleProfileRepository.GetsAsync(query =>
                query
                    .Include(c => c.Profile)
                    .Include(c => c.Role)
                    .Where(c => c.Role != null && c.Profile != null)
            );

            var roleProfiles = new List<RoleProfile>();
            var updatedRoleCodes = new HashSet<string>();
            var roleProfileDict = existingRoleProfiles.ToDictionary(c => new { c.RoleId, c.ProfileId }, c => c);

            var resultList = new List<string>();

            foreach (var role in userRoles)
            {
                if (profileDict.TryGetValue(role.Code, out var profile) && profile != null)
                {
                    var roleProfileKey = new { RoleId = role.Id, ProfileId = profile.Id };

                    if (!roleProfileDict.ContainsKey(roleProfileKey))
                    {
                        updatedRoleCodes.Add(role.Code);
                        roleProfiles.Add(new RoleProfile
                        {
                            Id = Guid.NewGuid(),
                            RoleId = role.Id,
                            ProfileId = profile.Id
                        });
                    }
                    else
                    {
                        roleProfileDict.Remove(roleProfileKey);
                    }

                    resultList.Add($"{profile.Name}");
                }
            }

            foreach (var roleProfile in roleProfileDict.Values)
            {
                roleProfile.IsActive = false;
                roleProfile.DeletedDate = DateTimeOffset.UtcNow;
            }
            await _roleProfileRepository.CreateRangeAsync(roleProfiles);
            await _roleProfileRepository.UpdateRangeAsync(existingRoleProfiles.Where(q => !q.IsActive).ToList());

            // Update role cache when there are changes
            if (updatedRoleCodes.Count > 0)
            {
                await _synchronizeCache.ClearKeys(updatedRoleCodes);
                await _synchronizeCache.GenerateRoleKeys(updatedRoleCodes);
            }

            return resultList;
        }
        private async Task<(User?, List<string>)> SynchronizeUser(string email, string name, string username, List<Role> roles)
        {
            User? user = null;

            var existUser = await _userRepository.GetByIdentifiersAsync(new Dictionary<string, object>
            {
                { "email", email },
                { "isactive", true }
            });

            var roleDict = roles.ToDictionary(role => role.Code, _ => false);
            var developers = new List<Developer>();
            var developerUserTypes = new List<DeveloperUserType>();
            var employees = new List<Employee>();
            var employeeUserTypes = new List<EmployeeUserType>();

            if (existUser == null)
            {
                var newUser = await CreateNewUser(email, name, username);
                user = newUser;

                foreach (var role in roles)
                {
                    switch (role.UserType?.Code)
                    {
                        case nameof(DeveloperUserType):
                            var developer = InitializeDeveloper(newUser.Name);
                            developers.Add(developer);
                            developerUserTypes.Add(InitializeDeveloperUserType(newUser, role, developer.Id));
                            roleDict[role.Code] = true;
                            break;
                        case nameof(EmployeeUserType):
                            var employee = InitializeEmployee(newUser.Name);
                            employees.Add(employee);
                            employeeUserTypes.Add(InitializeEmployeeUserType(newUser, role, employee.Id));
                            roleDict[role.Code] = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                user = existUser;
                var existingRoles = await _userUserTypeRepository.GetsAsync(query => query.Where(c => c.UserId == existUser.Id && c.IsEnabled && c.RoleId.HasValue));
                var existingRoleDict = existingRoles.ToDictionary(c => c.RoleId!.Value, c => true);

                foreach (var role in roles)
                {
                    if (existingRoleDict.ContainsKey(role.Id))
                    {
                        roleDict[role.Code] = true;
                        continue;
                    }

                    switch (role.UserType?.Code)
                    {
                        case nameof(DeveloperUserType):
                            var existingDeveloperUserTypes = await _developerUserTypeRepository.GetsAsync(query => query.Where(c => c.UserId == existUser.Id));
                            Guid? developerId = existingDeveloperUserTypes.FirstOrDefault()?.DeveloperId;
                            if (developerId == null)
                            {
                                var developer = InitializeDeveloper(existUser.Name);
                                developers.Add(developer);
                                developerId = developer.Id;
                            }
                            developerUserTypes.Add(InitializeDeveloperUserType(existUser, role, developerId));
                            roleDict[role.Code] = true;
                            break;
                        case nameof(EmployeeUserType):
                            var existingEmployeeUserTypes = await _employeeUserTypeRepository.GetsAsync(query => query.Where(c => c.UserId == existUser.Id));
                            Guid? employeeId = existingEmployeeUserTypes.FirstOrDefault()?.EmployeeId;
                            if (employeeId == null)
                            {
                                var employee = InitializeEmployee(existUser.Name);
                                employees.Add(employee);
                                employeeId = employee.Id;
                            }
                            employeeUserTypes.Add(InitializeEmployeeUserType(existUser, role, employeeId));
                            roleDict[role.Code] = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            await _developerRepository.CreateRangeAsync(developers);
            await _developerUserTypeRepository.CreateRangeAsync(developerUserTypes);
            await _employeeRepository.CreateRangeAsync(employees);
            await _employeeUserTypeRepository.CreateRangeAsync(employeeUserTypes);

            var validRoles = roleDict.Where(entry => entry.Value).Select(entry => entry.Key).ToList();
            // await SynchronizeRole(user?.Id, validRoles);

            return (user, validRoles);
        }
        private async Task SynchronizeRole(Guid? userId, List<string> roles)
        {
            var validRoles = await _roleRepository.GetsByCodeAsync(roles);
            var validRoleIds = validRoles.Select(c => c.Id).ToHashSet();

            var existingRoles = await _userUserTypeRepository.GetsAsync(query =>
                query.Where(c => c.UserId == userId && c.IsEnabled && c.RoleId.HasValue));

            var rolesToDelete = existingRoles
                .Where(existingRole => !validRoleIds.Contains(existingRole.RoleId!.Value))
                .ToList();

            if (rolesToDelete.Any())
            {
                foreach (var role in rolesToDelete)
                {
                    role.IsActive = false;
                }
                await _userUserTypeRepository.UpdateRangeAsync(rolesToDelete);
            }
        }
        private DeveloperUserType InitializeDeveloperUserType(User user, Role role, Guid? developerId)
        {
            var developerUserType = new DeveloperUserType
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                UserTypeId = role.UserTypeId,
                RoleId = role.Id,
                CreatedBy = "System",
                DeveloperId = developerId
            };
            return developerUserType;
        }
        private Developer InitializeDeveloper(string name)
        {
            var developer = new Developer
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedBy = "System"
            };
            return developer;
        }
        private EmployeeUserType InitializeEmployeeUserType(User user, Role role, Guid? employeeId)
        {
            var employeeUserType = new EmployeeUserType
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                UserTypeId = role.UserTypeId,
                RoleId = role.Id,
                CreatedBy = "System",
                EmployeeId = employeeId
            };
            return employeeUserType;
        }
        private Employee InitializeEmployee(string name)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = name,
                Code = "emp",
                CreatedBy = "System",
                GenderCode = "m",
                Phone = "+62 1111 1111 1111",
                Address = "-",
                BirthDate = new DateOnly(2000, 1, 1),
                HiredDate = new DateOnly(2022, 1, 1),
            };
            return employee;
        }
        private async Task<User> CreateNewUser(string email, string name, string username)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Username = username,
                CreatedBy = "System"
            };
            await _userRepository.CreateAsync(user);
            return user;
        }
    }
}
