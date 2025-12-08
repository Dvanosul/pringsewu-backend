using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Authentication.Services;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Models;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.app015.Application.Services;
using Sindika.AspNet.app015.Application.Services.Blockchain;
using Sindika.AspNet.app015.Infrastructure.DataContext;

namespace Sindika.AspNet.app015.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<TransactionOption>();
            services.AddScoped(typeof(ICacheService), typeof(CacheService<Context>));
            services.AddScoped<ISynchronizeCache, SynchronizeCache<Context>>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRoleProfileService, RoleProfileService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICustomEventService, CustomEventService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IZoneService, ZoneService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeveloperRoleService, DeveloperRoleService>();
            services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IUserInfoService, UserInfoService>();

            services.AddScoped<IGenderService, GenderService>();

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<ISubDistrictService, SubDistrictService>();

            services.AddScoped<ILocationService, LocationService>();

            // Donation Event and Gallery Services (PostgreSQL-based)
            services.AddScoped<IDonationEventService, DonationEventService>();
            services.AddScoped<IDonationGalleryService, DonationGalleryService>();

            // Blockchain Services - communicate with VaFund API (for Donations and Withdrawals only)
            services.AddScoped<IBlockchainDonationService, BlockchainDonationService>();
            services.AddScoped<IBlockchainWithdrawalService, BlockchainWithdrawalService>();

            // Pending Donation Service - uses Redis
            services.AddScoped<IPendingDonationService, PendingDonationService>();

            // Midtrans Payment Service
            services.AddScoped<IMidtransService, MidtransService>();

            return services;
        }
    }
}
