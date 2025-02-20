using Yugen.HomeBudget.Client.Helpers;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Models;

public class Category
{
    public Category()
    {
    }

    public Category(
        int id,
        string title,
        string icon,
        List<SubCategoryDto> subCategoriesDto)
    {
        Id = id;
        Title = title;
        Icon = IconHelper.GetIconName(icon);
        SubCategoriesDto = subCategoriesDto;
    }

    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public IconName Icon { get; set; } = IconName.Bold;

    public string IconString => Icon.ToString();

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = new List<SubCategoryDto>();
}