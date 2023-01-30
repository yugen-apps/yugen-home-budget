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
		Icon = icon;
		SubCategoriesDto = subCategoriesDto;
	}

	public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

	public List<SubCategoryDto> SubCategoriesDto { get; set; } = new List<SubCategoryDto>();
}