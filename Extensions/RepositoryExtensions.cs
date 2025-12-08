using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Infrastructure.Repositories;

namespace Sindika.AspNet.app015.Extensions
{
    // Extension method to add repositories to the service collection
    public static class ServiceExteRepositoryExtensionsnsions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserUserTypeRepository, UserUserTypeRepository>();
            services.AddScoped<IDeveloperUserTypeRepository, DeveloperUserTypeRepository>();
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageEventRepository, PageEventRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IProfilePageEventRepository, ProfilePageEventRepository>();
            services.AddScoped<IRoleProfileRepository, RoleProfileRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeUserTypeRepository, EmployeeUserTypeRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuPageEventRepository, MenuPageEventRepository>();

            services.AddScoped<IGenderRepository, GenderRepository>();

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<ISubDistrictRepository, SubDistrictRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDonationEventRepository, DonationEventRepository>();
            services.AddScoped<IDonationGalleryRepository, DonationGalleryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
