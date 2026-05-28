namespace Yugen.Home.Budget.Application.Models.Expense;

public class ResponseExpenseGroupedByCategoryDto
{
	public ResponseExpenseGroupedByCategoryDto(
		string category,
		int total,
		int index)
	{
		Category = category;
		Total = total;
		Index = index;
	}

	public string Category { get; set; }

	public int Total { get; set; }

	public int Index { get; set; }
}