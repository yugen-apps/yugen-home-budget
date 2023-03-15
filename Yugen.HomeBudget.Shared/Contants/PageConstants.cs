namespace Yugen.HomeBudget.Shared.Contants
{
    public static class PageConstants
    {
        public const string CategoryTitle = "Category";

        public const string CategoryUrl = "category";

        public const string CategoryAddUrl = $"{CategoryUrl}/addedit";

        public const string CategoryEditUrl = $"{CategoryUrl}/addedit/{{Id:int}}";

        public const string ExpenseTitle = "Expense";

        public const string ExpenseUrl = "expense";

        public const string ExpenseAddUrl = $"{ExpenseUrl}/addedit";

        public const string ExpenseEditUrl = $"{ExpenseUrl}/addedit/{{Id:int}}";
        
        public const string Home = "Home";

        public const string HomeUrl = "/";

        public const string Login = "Login";

        public const string Register = "Register";

        public const string Authentication = "authentication";

        public const string LoginUrl = $"{Authentication}/login";

        public const string RegisterUrl = $"{Authentication}/register";
    }
}