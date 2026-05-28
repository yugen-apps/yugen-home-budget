using System.Collections.Generic;

namespace Yugen.Home.Budget.Application.Models.Category;

public class UpdateCategoryDto
{
	public UpdateCategoryDto(
		int id,
		string title,
		string icon,
		List<SubCategoryDto> subCategoriesDto,
		int createdByApplicationUserId,
		int lastModifiedByApplicationUserId)
	{
		Id = id;
		Title = title;
		Icon = icon;
		SubCategoriesDto = subCategoriesDto;
		CreatedByApplicationUserId = createdByApplicationUserId;
		LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
	}

	public int Id { get; set; }

	public string Title { get; set; }

	public string Icon { get; set; }

	public List<SubCategoryDto> SubCategoriesDto { get; set; }

	public int CreatedByApplicationUserId { get; set; }

	public int LastModifiedByApplicationUserId { get; set; }
}