using System;
using System.Collections.Generic;

namespace Yugen.Home.Budget.Application.Models.Category;

public class ResponseCategoryDto
{
    public ResponseCategoryDto(
        int id,
        string title,
        string icon,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn,
        int? createdByApplicationUserId,
        int? lastModifiedByApplicationUserId)
    {
        Id = id;
        Title = title;
        IconName = icon;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public string IconName { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = [];

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastModifiedOn { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public int? LastModifiedByApplicationUserId { get; set; }
}