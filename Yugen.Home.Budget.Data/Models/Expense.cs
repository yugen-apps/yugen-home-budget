using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yugen.Home.Budget.Data.Models;

public class Expense : Entity
{
	public Expense()
	{
	}

	public Expense(
		string title,
		decimal amount,
		DateTimeOffset dateTimeOffset,
		int categoryId,
		int subCategoryId,
		decimal accrued,
		int createdByApplicationUserId)
	{
		Title = title;
		Amount = amount;
		DateTimeOffset = dateTimeOffset;
		CategoryId = categoryId;
		SubCategoryId = subCategoryId;
		Accrued = accrued;
		CreatedByApplicationUserId = createdByApplicationUserId;
	}

	[Column(TypeName = "decimal(18,2)")]
	public decimal Accrued { get; set; }

	[Column(TypeName = "decimal(18,2)")]
	public decimal Amount { get; set; }

	public DateTimeOffset DateTimeOffset { get; set; }

	public string Title { get; set; }

	public int? CategoryId { get; set; }

	public Category Category { get; set; }

	public int? SubCategoryId { get; set; }

	public SubCategory SubCategory { get; set; }
}