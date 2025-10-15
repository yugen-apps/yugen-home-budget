using System.Collections.Generic;
using Yugen.HomeBudget.Application.Models.Category;
using Yugen.HomeBudget.Server.Helpers.Icons;

namespace Yugen.HomeBudget.Server.Models.Category;

public class CategoryModel
{
    public CategoryModel()
    {
    }

    public CategoryModel(
        int id,
        string title,
        string icon,
        List<SubCategoryDto> subCategoriesDto)
    {
        Id = id;
        Title = title;
        IconName = IconHelper.GetIconName(icon);
        SubCategoriesDto = subCategoriesDto;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public string IconName { get; set; } = IconHelper.GetIconName(null);

    public string IconCode => IconHelper.GetIconCode(IconName);

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = [];
}