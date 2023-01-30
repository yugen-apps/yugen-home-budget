namespace Yugen.HomeBudget.Shared.Helpers;

public static class PageHelper
{
    public static string EditHref(string page, int id) => $"{page}/addedit/{id}";

    public static string AddEditTitle(string page, int? id) => id == null
        ? $"Add {page}"
        : $"Edit {page}";
}