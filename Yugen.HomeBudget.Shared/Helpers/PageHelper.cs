using Yugen.HomeBudget.Shared.Enums;

namespace Yugen.HomeBudget.Shared.Helpers;

public static class PageHelper
{
    public static string ListHref(PageList pageList) => $"{pageList}/list";

    public static string AddHref(PageList pageList) => $"{pageList}/edit";

    public static string EditHref(PageList pageList, int id) => $"{pageList}/edit/{id}";

    public static string AuthHref(PageList pageList) => $"authentication/{pageList}";

    public static string AddEditTitle(PageList pageList, int? id) => id == null
        ? $"Add {pageList}"
        : $"Edit {pageList}";
}