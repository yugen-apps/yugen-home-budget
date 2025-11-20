using MudBlazor;
using System.Collections.Generic;

namespace Yugen.HomeBudget.Server.Navigation
{
    public static class MenuConstants
    {
        public const string HomePath = "/";

        public const string CategoryPath = "category";
        public const string CategoryDetailsPath = $"{CategoryPath}/details";
        public const string CategoryDetailsRoute = $"{CategoryDetailsPath}/{{Id:int}}";

        public const string ExpensePath = "expenses";
        public const string ExpenseDetailsPath = $"{ExpensePath}/details";
        public const string ExpenseDetailsRoute = $"{ExpenseDetailsPath}/{{Id:int}}";

        public const string InfoPath = "info";

        public const string ApiPrefix = "api";
        public const string ApiExport = $"{ApiPrefix}/export";

        public static List<MenuItem> MenuItems =>
        [
            HomeMenuItem,
            CategoryMenuItem,
            ExpenseMenuItem
        ];

        public static MenuItem HomeMenuItem => new()
        {
            Path = HomePath,
            Icon = Icons.Material.Filled.Dashboard,
            Title = "Home"
        };

        public static MenuItem CategoryMenuItem => new()
        {
            Path = CategoryPath,
            Icon = Icons.Material.Filled.List,
            Title = "Category"
        };

        public static MenuItem ExpenseMenuItem => new()
        {
            Path = ExpensePath,
            Icon = Icons.Material.Filled.Money,
            Title = "Expense"
        };

        public static string AddEditTitle(string page, int? id) => id == null
            ? $"Add {page}"
            : $"Edit {page}";
    }
}