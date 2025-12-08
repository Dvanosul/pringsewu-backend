using Microsoft.EntityFrameworkCore;
using Sindika.AspNet.Authentication.Data;
using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.Common.Entities;

namespace Sindika.AspNet.app015.Infrastructure.DataContext
{
    public class Context : AuthContext
    {

        public Context(DbContextOptions<AuthContext> options)
        : base(options)
        {

        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<CityType> CityTypes => Set<CityType>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<SubDistrict> SubDistricts => Set<SubDistrict>();

        public DbSet<DeveloperUserType> DeveloperUserTypes => Set<DeveloperUserType>();
        public DbSet<Developer> Developers => Set<Developer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Gender> Genders => Set<Gender>();
        public DbSet<EmployeeUserType> EmployeeUserTypes => Set<EmployeeUserType>();

        public DbSet<DonationEvent> DonationEvents => Set<DonationEvent>();
        public DbSet<DonationGallery> DonationGalleries => Set<DonationGallery>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeveloperUserType>().ToTable(USER_USERTYPE_TABLENAME);
            modelBuilder.Entity<DeveloperUserType>().HasIndex(c => new { c.DeveloperId });

            modelBuilder.Entity<EmployeeUserType>().ToTable(USER_USERTYPE_TABLENAME);
            modelBuilder.Entity<EmployeeUserType>().HasIndex(c => new { c.EmployeeId });

            modelBuilder.Entity<Developer>().HasIndex(c => new { c.IsActive, c.Code });
            modelBuilder.Entity<Employee>().HasIndex(c => new { c.IsActive, c.Code });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder
            //     .LogTo(Console.WriteLine); // Logs SQL to the console
        }
    }
}
