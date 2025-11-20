using Blazorise;

namespace Yugen.HomeBudget.Server.Navigation
{
    public static class MenuConstants
    {
        public const string HomePath = "/";

        public const string CategoriesPath = "Categories";
        public const string CategoryPath = $"{CategoriesPath}/addedit";
        public const string CategoryRoute = $"{CategoryPath}/{{Id:int}}";

        public const string ExpensesPath = "Expenses";
        public const string ExpensePath = $"{ExpensesPath}/addedit";
        public const string ExpenseRoute = $"{ExpensePath}/{{Id:int}}";

        public const string InfoPath = "info";

        public const string ApiPrefix = "api";
        public const string ApiExport = $"{ApiPrefix}/export";

        public static List<MenuItem> MenuItems =>
        [
            HomeMenuItem,
            CategoriesMenuItem,
            ExpensesMenuItem
        ];

        public static MenuItem HomeMenuItem => new()
        {
            Path = HomePath,
            Icon = IconName.Dashboard,
            Title = "Home"
        };

        public static MenuItem CategoriesMenuItem => new()
        {
            Path = CategoriesPath,
            Icon = IconName.List,
            Title = "Categories"
        };

        public static MenuItem ExpensesMenuItem => new()
        {
            Path = ExpensesPath,
            Icon = IconName.DollarSign,
            Title = "Expenses"
        };

        public static string AddEditTitle(string page, int? id) => id == null
            ? $"Add {page}"
            : $"Edit {page}";
    }
}