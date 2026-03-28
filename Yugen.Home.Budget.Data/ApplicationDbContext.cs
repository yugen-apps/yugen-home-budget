using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yugen.Home.Budget.Data.Models;

namespace Yugen.Home.Budget.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<SubCategory> SubCategories { get; set; } = default!;

        public DbSet<Expense> Expenses { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Category>().HasData(
            //    new Category { Id = 1, Title = "Flat", Icon = "" }
            //    );

            //modelBuilder.Entity<SubCategory>().HasData(
            //    new SubCategory { Id = 1, Title = "Rent", CategoryId = 1 },
            //    new SubCategory { Id = 2, Title = "Utility Bills", CategoryId = 1 },
            //    new SubCategory { Id = 4, Title = "Internet", CategoryId = 1 },
            //    new SubCategory { Id = 5, Title = "Cleaning", CategoryId = 1 },
            //    new SubCategory { Id = 6, Title = "Household items", CategoryId = 1 },
            //    new SubCategory { Id = 7, Title = "Other", CategoryId = 1 }
            //);
        }
    }
}