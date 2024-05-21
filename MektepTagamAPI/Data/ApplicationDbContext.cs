using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MektepTagamAPI.Authentication;
using MektepTagamAPI.Authenticate;
using MektepTagamAPI.Models;
using ApiAvtoMigNew.Models;

namespace MektepTagamAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<ModelCar> ModelCars { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
