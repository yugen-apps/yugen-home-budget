using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yugen.Home.Budget.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [InverseProperty(nameof(Category.CreatedByApplicationUser))]
        public ICollection<Category> CreatedByCategories { get; set; }

        [InverseProperty(nameof(Category.LastModifiedByApplicationUser))]
        public ICollection<Category> LastModifiedByCategories { get; set; }

        [InverseProperty(nameof(Expense.CreatedByApplicationUser))]
        public ICollection<Expense> CreatedByExpenses { get; set; }

        [InverseProperty(nameof(Expense.LastModifiedByApplicationUser))]
        public ICollection<Expense> LastModifiedByExpenses { get; set; }

        [InverseProperty(nameof(SubCategory.CreatedByApplicationUser))]
        public ICollection<SubCategory> CreatedBySubCategories { get; set; }

        [InverseProperty(nameof(SubCategory.LastModifiedByApplicationUser))]
        public ICollection<SubCategory> LastModifiedBySubCategories { get; set; }
    }
}