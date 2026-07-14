using Riok.Mapperly.Abstractions;
using Yugen.Home.Budget.Application.Models.Expense;

namespace Yugen.Home.Budget.Server.Controllers;

[Mapper]
public static partial class MapperExtensions
{
    [MapperIgnoreSource(nameof(ResponseExpenseDto.CategoryDto))]
    [MapperIgnoreSource(nameof(ResponseExpenseDto.CreatedByApplicationUserId))]
    [MapperIgnoreSource(nameof(ResponseExpenseDto.CreatedOn))]
    [MapperIgnoreSource(nameof(ResponseExpenseDto.LastModifiedByApplicationUserId))]
    [MapperIgnoreSource(nameof(ResponseExpenseDto.LastModifiedOn))]
    [MapperIgnoreSource(nameof(ResponseExpenseDto.SubCategoryDto))]
    [MapProperty(nameof(ResponseExpenseDto.DateTimeOffset), nameof(ExportDto.DateTime))]
    public static partial ExportDto ToExportDto(this ResponseExpenseDto responseExpenseDto, string category, string subCategory);
}