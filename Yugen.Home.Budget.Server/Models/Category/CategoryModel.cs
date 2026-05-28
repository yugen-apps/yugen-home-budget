using System.Collections.Generic;
using Yugen.Home.Budget.Application.Models.Category;
using Yugen.Home.Budget.Server.Helpers.Icons;

namespace Yugen.Home.Budget.Server.Models.Category;

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