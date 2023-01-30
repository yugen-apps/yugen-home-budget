namespace Yugen.HomeBudget.Shared.Contants;

public static class EndpointConstants
{
    public const string Prefix = "api";

    public const string Category = $"{Prefix}/category";

    public const string Expense = $"{Prefix}/expense";

    public const string ExpenseSum = $"{Prefix}/expense/sum";

    public const string AccruedSum = $"{Prefix}/expense/sumaccrued";

    public const string GroupedByCategory = $"{Prefix}/expense/groupedbycategory";

    public const string Authentication = $"{Prefix}/authentication";

    public const string Export = $"{Prefix}/export";

    public const string Info = $"{Prefix}/info";
}