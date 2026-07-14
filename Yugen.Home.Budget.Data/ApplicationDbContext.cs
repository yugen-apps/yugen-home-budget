using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Yugen.Home.Budget.Data.Models;

namespace Yugen.Home.Budget.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<SubCategory> SubCategories { get; set; } = default!;

        public DbSet<Expense> Expenses { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Category>().HasData(
            //    new Category { Id = 1, Title = "Default", Icon = "" }
            //    );

            //modelBuilder.Entity<SubCategory>().HasData(
            //    new SubCategory { Id = 1, Title = "Default", CategoryId = 1 }
            //);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    if (!await context.Set<Category>().AnyAsync())
                    {
                        var defaultSubCategory = context.Set<SubCategory>().Add(DefaultSubCategory);
                        context.Set<Category>().Add(Category(defaultSubCategory.Entity));
                        await context.SaveChangesAsync(cancellationToken);
                    }
                })
                .UseSeeding(async (context, _) =>
                {
                    if (!context.Set<Category>().Any())
                    {
                        var defaultSubCategory = context.Set<SubCategory>().Add(DefaultSubCategory);
                        context.Set<Category>().Add(Category(defaultSubCategory.Entity));
                        context.SaveChanges();
                    }
                });
        }

        private static SubCategory DefaultSubCategory => new()
        {
            Title = "Default"
        };

        private static Category Category(SubCategory subCategory) => new()
        {
            Title = "Default",
            Icon = string.Empty,
            SubCategories = new[] { subCategory }
        };
    }
}