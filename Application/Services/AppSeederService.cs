using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.app015.Application.Seeders;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Authentication.Interfaces;
using Sindika.AspNet.Common.Entities;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Services
{
    public class AppSeederService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppSeederService> _logger;

        public AppSeederService(IServiceProvider serviceProvider, ILogger<AppSeederService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<Context>>();
                var synchronizeCache = scope.ServiceProvider.GetRequiredService<ISynchronizeCache>();
                var context = unitOfWork.GetContext();
                await unitOfWork.BeginTransactionAsync();
                try
                {
                    await SeedCountries(context);
                    await SeedProvinces(context);
                    await SeedCityTypes(context);
                    await SeedCities(context);
                    await SeedDistricts(context);
                    await SeedSubDistricts(context);

                    await SeedUserTypes(context);
                    await SeedRoles(context);
                    await SeedProfiles(context);
                    await SeedRoleProfile(context);
                    await SeedMenu(context);
                    await SeedGenders(context);
                    await SeedMenuPageEvent(context);
                    
                    await unitOfWork.CommitAsync();
                }
                catch (System.Exception)
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }

                await synchronizeCache.ClearKeys();
                await synchronizeCache.GenerateRoleKeys();
            }
        }

        private async Task SeedProfiles(Context context)
        {
            var profilePermissions = new Dictionary<string, string[]>
            {
                {
                    "super-admin",
                    new[]
                    {
                        "EXAMPLE", "AUTH", "LIMITATION", "PROFILE", "ROLE", "ROLEPROFILE", "PAGE", "EVENT",
                        "CUSTOMEVENT", "FILE", "DEVELOPER", "LANGUAGE", "ZONE", "GENDER", "EMPLOYEE", "USERTYPE",
                        "USER", "DEVELOPERROLE", "EMPLOYEEROLE"
                    }
                },
            };

            var profiles = new[]
            {
                new Profile
                {
                    Id = Guid.Parse("891f1cf4-1031-48a6-96c2-e6ec27dfdecc"), Code = "super-admin", Name = "Developer",
                    Description = "Developer Aplikasi"
                },
            };

            var insertedCount = 0;

            var existingProfilePageEvents = context.ProfilePageEvents
                .Include(c => c.Page)
                .Include(c => c.Profile)
                .Include(c => c.Event)
                .Where(c => c.Page != null && c.Profile != null && c.Event != null)
                .ToList();

            var existingProfilePageEventDict = existingProfilePageEvents
                .GroupBy(c => new { ProfileId = c.Profile!.Id, PageCode = c.Page!.Code, EventCode = c.Event!.Code })
                .ToDictionary(c => c.Key, c => c.FirstOrDefault());

            // Delete profile page events that have inactive event
            var profilePageEventsToDelete =
                await context.ProfilePageEvents.Where(c => c.IsActive && !c.Event!.IsActive).ToListAsync();
            profilePageEventsToDelete.ForEach(ppe =>
            {
                ppe.IsActive = false;
                ppe.DeletedDate = DateTimeOffset.UtcNow;
            });
            context.ProfilePageEvents.UpdateRange(profilePageEventsToDelete);

            foreach (var profile in profiles)
            {
                if (!context.Profiles.Any(l => l.Id == profile.Id))
                {
                    context.Profiles.Add(profile);
                    insertedCount++;
                }
            }

            if (insertedCount == 0)
            {
                return;
            }

            var pageEvents = context.PageEvents
                .Include(c => c.Event)
                .Include(c => c.Page)
                .ToList();

            var pageEventDict = pageEvents
                .Where(c => c.Page != null && c.Event != null && c.HasEvent)
                .GroupBy(c => c.Page!.Code)
                .ToDictionary(c => c.Key, c => c.ToList());

            var profilePageEvents = new List<ProfilePageEvent>();

            var totalEventCount = 0;
            var eventLogDetails = new List<string>();
            var existingKeys = new HashSet<object>();
            int eventsAddedCount = 0;

            foreach (var profile in profiles)
            {
                var profileEventCount = 0;
                int profileEventsAdded = 0;

                if (profilePermissions.TryGetValue(profile.Code, out var pages))
                {
                    foreach (var page in pages)
                    {
                        if (pageEventDict.TryGetValue(page, out var events))
                        {
                            foreach (var evnt in events)
                            {
                                if (evnt != null)
                                {
                                    var key = new
                                    { ProfileId = profile.Id, PageCode = page, EventCode = evnt.Event!.Code };
                                    if (!existingProfilePageEventDict.ContainsKey(key))
                                    {
                                        profilePageEvents.Add(new ProfilePageEvent
                                        {
                                            Id = Guid.NewGuid(),
                                            ProfileId = profile.Id,
                                            EventId = evnt.EventId,
                                            PageId = evnt.PageId
                                        });
                                        profileEventCount++;
                                        profileEventsAdded++;
                                        eventsAddedCount++;
                                    }
                                    else
                                    {
                                        existingKeys.Add(key);
                                    }
                                }
                            }
                        }
                    }
                }

                totalEventCount += profileEventCount;
                eventLogDetails.Add($"Profile '{profile.Code}' ({profile.Name}) -> Added: {profileEventsAdded} events");
            }

            if (profilePageEvents.Any())
            {
                await context.ProfilePageEvents.AddRangeAsync(profilePageEvents);
            }

            await context.SaveChangesAsync();

            _logger.LogInformation($"Profiles seed {insertedCount} data");
            _logger.LogInformation($"Total events generated: {totalEventCount}");

            eventLogDetails.Add($"Total events added: {eventsAddedCount}");

            foreach (var logDetail in eventLogDetails)
            {
                _logger.LogInformation(logDetail);
            }
        }

        private async Task SeedUserTypes(Context context)
        {
            var userTypes = new[]
            {
                new UserType
                {
                    Id = Guid.Parse("c67cd479-141c-471c-b0d3-94b2be7e9fd8"), Code = "DeveloperUserType",
                    Name = "Developer"
                },
                new UserType
                {
                    Id = Guid.Parse("86500e6c-cc19-4dea-b8ca-6902554700b0"), Code = "EmployeeUserType",
                    Name = "Employee"
                },
            };

            var insertedCount = 0;

            foreach (var userType in userTypes)
            {
                if (!context.UserTypes.Any(l => l.Id == userType.Id))
                {
                    context.UserTypes.Add(userType);
                    insertedCount++;
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"UserTypes seed {insertedCount} data");
        }

        private async Task SeedRoles(Context context)
        {
            var roles = new[]
            {
                new Role
                {
                    Id = Guid.Parse("b9faf362-1890-41ed-b088-7e9d23fd097b"), Code = "super-admin", Name = "Super Admin",
                    UserTypeId = Guid.Parse("c67cd479-141c-471c-b0d3-94b2be7e9fd8")
                },
                new Role
                {
                    Id = Guid.Parse("b6c2d5a1-1f0e-46a3-9c81-39e8e3a44f21"), Code = "karyawan", Name = "Karyawan",
                    UserTypeId = Guid.Parse("86500e6c-cc19-4dea-b8ca-6902554700b0")
                },
            };

            var insertedCount = 0;

            foreach (var role in roles)
            {
                if (!context.Roles.Any(l => l.Id == role.Id))
                {
                    context.Roles.Add(role);
                    insertedCount++;
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Roles seed {insertedCount} data");
        }

        private async Task SeedRoleProfile(Context context)
        {
            var roleProfiles = new[]
            {
                new RoleProfile
                {
                    Id = Guid.Parse("71fb253b-1797-4768-9602-96e6f8fa29ad"),
                    RoleId = Guid.Parse("b9faf362-1890-41ed-b088-7e9d23fd097b"),
                    ProfileId = Guid.Parse("891f1cf4-1031-48a6-96c2-e6ec27dfdecc")
                },
            };

            var insertedCount = 0;

            foreach (var roleProfile in roleProfiles)
            {
                if (!context.RoleProfiles.Any(l => l.Id == roleProfile.Id))
                {
                    context.RoleProfiles.Add(roleProfile);
                    insertedCount++;
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"RoleProfiles seed {insertedCount} data");
        }

        private async Task SeedMenu(Context context)
        {
            var menus = new[]
            {
                new Menu
                {
                    Id = Guid.Parse("80463cfd-d42a-4eaf-ae86-ea7ea34add67"), Code = "main-menu", Number = "1",
                    InternalCode = "001", Name = "MAIN_MENU", IsExpanded = true
                },
                new Menu
                {
                    Id = Guid.Parse("f21d0b48-cc0d-49d7-b72a-3fda9cc6e19c"), Code = "master", Number = "1.1",
                    InternalCode = "001:001", Icon = "ki-category", Name = "MASTER", IsExpanded = true
                },
                new Menu
                {
                    Id = Guid.Parse("f384b2e8-8af1-4e37-8f87-e02d1ee12451"), Code = "administration", Number = "1.4",
                    InternalCode = "001:004", Icon = "ki-key", Name = "ADMINISTRATION", IsExpanded = true
                },
                new Menu
                {
                    Id = Guid.Parse("63db7910-8ab4-4ae6-ac8b-008a32ae526c"), Code = "role", Number = "1.4.1",
                    InternalCode = "001:004:001", Name = "ADMINISTRATION.ROLE"
                },
                new Menu
                {
                    Id = Guid.Parse("a924dfbc-9cc2-406e-b970-cf1834a52d29"), Code = "profile", Number = "1.4.2",
                    InternalCode = "001:004:002", Name = "ADMINISTRATION.PROFILE"
                },
                new Menu
                {
                    Id = Guid.Parse("5e414957-8ed6-4a85-b8ab-4e45a9dc0abb"), Code = "custom-event", Number = "1.4.3",
                    InternalCode = "001:004:003", Name = "ADMINISTRATION.CUSTOM_EVENT"
                },
                new Menu
                {
                    Id = Guid.Parse("e8caa992-0db4-4f2e-a20c-a10d93808e55"), Code = "user-type", Number = "1.5",
                    InternalCode = "001:005", Icon = "ki-speaker", Name = "USER_TYPE", IsExpanded = true
                },
                new Menu
                {
                    Id = Guid.Parse("2202e1d0-ef46-4c40-8cc0-d6d89b60c4f6"), Code = "employee", Number = "1.5.1",
                    InternalCode = "001:005:001", Name = "USER_TYPE.EMPLOYEE"
                },
                new Menu
                {
                    Id = Guid.Parse("ef50bf2d-8299-4e67-93c0-2e6ee7385ffd"), Code = "developer", Number = "1.5.2",
                    InternalCode = "001:005:002", Name = "USER_TYPE.DEVELOPER"
                },
                new Menu
                {
                    Id = Guid.Parse("900cd53c-821d-45ca-babd-6939334d7356"), Code = "user", Number = "1.5.3",
                    InternalCode = "001:005:003", Name = "USER_TYPE.USER"
                },

                new Menu
                {
                    Id = Guid.Parse("2df793cf-74e1-40e7-bf58-29e8bb34551e"), Code = "menu-management", Number = "1.6",
                    InternalCode = "001:006", Icon = "shield-tick", Name = "MENU_MANAGEMENT"
                },
            };

            var insertedCount = 0;

            foreach (var menu in menus)
            {
                if (!context.Menus.Any(l => l.Id == menu.Id))
                {
                    context.Menus.Add(menu);
                    insertedCount++;
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Menus seed {insertedCount} data");
        }

        private async Task SeedMenuPageEvent(Context context)
        {
            var menuPageEvents = new Dictionary<string, (string, string)>
            {
                { "role", ("ROLE", "view") },
                { "profile", ("PROFILE", "view") },
                { "custom-event", ("CUSTOMEVENT", "view") },
                { "employee", ("EMPLOYEE", "view") },
                { "developer", ("DEVELOPER", "view") },
                { "user", ("USER", "view") },
                { "menu-management", ("MENU", "view") },
            };

            var insertedCount = 0;

            foreach (var menuPageEvent in menuPageEvents)
            {
                var menuId = context.Menus
                    .Where(m => m.IsActive && m.Code == menuPageEvent.Key)
                    .Select(m => m.Id)
                    .FirstOrDefault();

                var pageEventId = context.PageEvents
                    .Where(pe =>
                        pe.IsActive && pe.Page!.Code == menuPageEvent.Value.Item1 &&
                        pe.Event!.Code == menuPageEvent.Value.Item2)
                    .Select(pe => pe.Id)
                    .FirstOrDefault();

                if (menuId == Guid.Empty || pageEventId == Guid.Empty)
                {
                    _logger.LogWarning(
                        $"Skipping insert: MenuId or PageEventId not found for {menuPageEvent.Key} -> {menuPageEvent.Value}");
                    continue;
                }

                if (!context.MenuPageEvents.Any(l => l.MenuId == menuId && l.PageEventId == pageEventId))
                {
                    context.MenuPageEvents.Add(new MenuPageEvent { MenuId = menuId, PageEventId = pageEventId });
                    insertedCount++;
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Menu Page Event seed {insertedCount} data");
        }

        private async Task SeedGenders(Context context)
        {
            var genders = GenderSeeder.GetGenders();
            var existingIds = new HashSet<Guid>(context.Genders.Select(ct => ct.Id));
            var newGenders = genders.Where(ct => !existingIds.Contains(ct.Id)).ToList();

            if (newGenders.Any())
            {
                await AddEntitiesInBatch(context, newGenders);
                _logger.LogInformation($"Seeded {newGenders.Count} Genders.");
            }
        }

        private async Task SeedCountries(Context context)
        {
            var countries = CountrySeeder.GetCountries();
            var existingIds = new HashSet<Guid>(context.Countries.Select(c => c.Id));
            var newCountries = countries.Where(c => !existingIds.Contains(c.Id)).ToList();

            if (newCountries.Any())
            {
                await AddEntitiesInBatch(context, newCountries);
                _logger.LogInformation($"Seeded {newCountries.Count} Countries.");
            }
        }

        private async Task SeedProvinces(Context context)
        {
            var provinces = ProvinceSeeder.GetProvinces();
            var existingIds = new HashSet<Guid>(context.Provinces.Select(p => p.Id));
            var newProvinces = provinces.Where(p => !existingIds.Contains(p.Id)).ToList();

            if (newProvinces.Any())
            {
                await AddEntitiesInBatch(context, newProvinces);
                _logger.LogInformation($"Seeded {newProvinces.Count} Provinces.");
            }
        }

        private async Task SeedCityTypes(Context context)
        {
            var cityTypes = CityTypeSeeder.GetCityTypes();
            var existingIds = new HashSet<Guid>(context.CityTypes.Select(ct => ct.Id));
            var newCityTypes = cityTypes.Where(ct => !existingIds.Contains(ct.Id)).ToList();

            if (newCityTypes.Any())
            {
                await AddEntitiesInBatch(context, newCityTypes);
                _logger.LogInformation($"Seeded {newCityTypes.Count} City Types.");
            }
        }

        private async Task SeedCities(Context context)
        {
            var cities = CitySeeder.GetCities();
            var existingIds = new HashSet<Guid>(context.Cities.Select(c => c.Id));
            var newCities = cities.Where(c => !existingIds.Contains(c.Id)).ToList();

            if (newCities.Any())
            {
                await AddEntitiesInBatch(context, newCities);
                _logger.LogInformation($"Seeded {newCities.Count} Cities.");
            }
        }

        private async Task SeedDistricts(Context context)
        {
            var districts = DistrictSeeder.GetDistricts();
            var existingIds = new HashSet<Guid>(context.Districts.Select(d => d.Id));
            var newDistricts = districts.Where(d => !existingIds.Contains(d.Id)).ToList();

            if (newDistricts.Any())
            {
                await AddEntitiesInBatch(context, newDistricts);
                _logger.LogInformation($"Seeded {newDistricts.Count} Districts.");
            }
        }

        private async Task SeedSubDistricts(Context context)
        {
            var subDistricts = SubDistrictSeeder.GetSubDistricts();
            var existingIds = new HashSet<Guid>(context.SubDistricts.Select(sd => sd.Id));
            var newSubDistricts = subDistricts.Where(sd => !existingIds.Contains(sd.Id)).ToList();

            if (newSubDistricts.Any())
            {
                await AddEntitiesInBatch(context, newSubDistricts);
                _logger.LogInformation($"Seeded {newSubDistricts.Count} SubDistricts.");
            }
        }

        private async Task AddEntitiesInBatch<T>(Context context, List<T> entities)
        where T : class
        {
            const int batchSize = 1000;
            for (int i = 0; i < entities.Count; i += batchSize)
            {
                var batch = entities.Skip(i).Take(batchSize).ToList();
                context.AddRange(batch);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
