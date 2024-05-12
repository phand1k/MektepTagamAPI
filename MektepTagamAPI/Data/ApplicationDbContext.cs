using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MektepTagamAPI.Authentication;
using MektepTagamAPI.Authenticate;
using MektepTagamAPI.Models;

namespace MektepTagamAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<CardCode> CardCodes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
