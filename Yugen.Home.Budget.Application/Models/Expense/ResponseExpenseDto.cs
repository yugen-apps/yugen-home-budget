using System;
using Yugen.Home.Budget.Application.Models.Category;

namespace Yugen.Home.Budget.Application.Models.Expense;

public class ResponseExpenseDto
{
	public ResponseExpenseDto(
		int id,
		string title,
		decimal amount,
		DateTimeOffset dateTimeOffset,
		ResponseCategoryDto categoryDto,
		SubCategoryDto subCategoryDto,
		decimal accrued,
		DateTimeOffset createdOn,
		DateTimeOffset lastModifiedOn,
		int? createdByApplicationUserId,
		int? lastModifiedByApplicationUserId)
	{
		Id = id;
		Title = title;
		Amount = amount;
		DateTimeOffset = dateTimeOffset;
		CategoryDto = categoryDto;
		SubCategoryDto = subCategoryDto;
		Accrued = accrued;
		CreatedOn = createdOn;
		LastModifiedOn = lastModifiedOn;
		CreatedByApplicationUserId = createdByApplicationUserId;
		LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
	}

	public decimal Accrued { get; set; }

	public decimal Amount { get; set; }

	public ResponseCategoryDto CategoryDto { get; set; }

	public int? CreatedByApplicationUserId { get; set; }

	public DateTimeOffset CreatedOn { get; set; }

	public DateTimeOffset DateTimeOffset { get; set; }

	public int Id { get; set; }

	public int? LastModifiedByApplicationUserId { get; set; }

	public DateTimeOffset LastModifiedOn { get; set; }

	public SubCategoryDto SubCategoryDto { get; set; }

	public string Title { get; set; }
}