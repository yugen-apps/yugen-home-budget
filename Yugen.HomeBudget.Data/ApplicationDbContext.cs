using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yugen.HomeBudget.Data.Models;

namespace Yugen.HomeBudget.Data
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

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Title = "Vienna Flat", Icon = "" },
                new Category { Id = 2, Title = "Vienna Car", Icon = "" },
                new Category { Id = 3, Title = "Riga Flat", Icon = "" },
                new Category { Id = 4, Title = "Balozi Flat", Icon = "" },
                new Category { Id = 5, Title = "Klaus", Icon = "" },
                new Category { Id = 6, Title = "Food", Icon = "" },
                new Category { Id = 7, Title = "Clothing", Icon = "" },
                new Category { Id = 8, Title = "Entertainment", Icon = "" },
                new Category { Id = 9, Title = "Travel", Icon = "" },
                new Category { Id = 10, Title = "Education", Icon = "" },
                new Category { Id = 11, Title = "Health", Icon = "" }
                );

            modelBuilder.Entity<SubCategory>().HasData(
                new SubCategory { Id = 1, Title = "Rent", CategoryId = 1},
                new SubCategory { Id = 2, Title = "Utility Bills", CategoryId = 1 },
                new SubCategory { Id = 3, Title = "GIS", CategoryId = 1 },
                new SubCategory { Id = 4, Title = "Internet", CategoryId = 1 },
                new SubCategory { Id = 5, Title = "Cleaning", CategoryId = 1 },
                new SubCategory { Id = 6, Title = "Household items", CategoryId = 1 },
                new SubCategory { Id = 7, Title = "Other", CategoryId = 1 },
                new SubCategory { Id = 8, Title = "Leasing / Rent", CategoryId = 2 },
                new SubCategory { Id = 9, Title = "Parking", CategoryId = 2 },
                new SubCategory { Id = 10, Title = "Charging / Gasoline", CategoryId = 2 },
                new SubCategory { Id = 11, Title = "Other", CategoryId = 2 },
                new SubCategory { Id = 12, Title = "Utility Bills", CategoryId = 3 },
                new SubCategory { Id = 13, Title = "Household items", CategoryId = 3 },
                new SubCategory { Id = 14, Title = "Other", CategoryId = 3 },
                new SubCategory { Id = 15, Title = "Utility Bills", CategoryId = 4 },
                new SubCategory { Id = 16, Title = "Household items", CategoryId = 4 },
                new SubCategory { Id = 17, Title = "Other", CategoryId = 4 },
                new SubCategory { Id = 18, Title = "Food", CategoryId = 5 },
                new SubCategory { Id = 19, Title = "Vet", CategoryId = 5 },
                new SubCategory { Id = 20, Title = "Dog Sitter", CategoryId = 5 },
                new SubCategory { Id = 21, Title = "Other", CategoryId = 5 },
                new SubCategory { Id = 22, Title = "Groceries", CategoryId = 6 },
                new SubCategory { Id = 23, Title = "Eating out", CategoryId = 6 },
                new SubCategory { Id = 24, Title = "Ilva", CategoryId = 7 },
                new SubCategory { Id = 25, Title = "Emil", CategoryId = 7 },
                new SubCategory { Id = 26, Title = "Tickets Concerts", CategoryId = 8 },
                new SubCategory { Id = 27, Title = "Presents", CategoryId = 8 },
                new SubCategory { Id = 28, Title = "Other", CategoryId = 8 },
                new SubCategory { Id = 29, Title = "Transport", CategoryId = 9 },
                new SubCategory { Id = 30, Title = "Accomodation", CategoryId = 9 },
                new SubCategory { Id = 31, Title = "Other", CategoryId = 9 },
                new SubCategory { Id = 32, Title = "Ilva", CategoryId = 10 },
                new SubCategory { Id = 33, Title = "Emil", CategoryId = 10 },
                new SubCategory { Id = 34, Title = "Sport Ilva", CategoryId = 11 },
                new SubCategory { Id = 35, Title = "Sport Emil", CategoryId = 11 },
                new SubCategory { Id = 36, Title = "Med Ilva", CategoryId = 11 },
                new SubCategory { Id = 37, Title = "Med Emil", CategoryId = 11 }
            );
        }
    }
}