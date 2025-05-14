namespace Yugen.HomeBudget.Shared.Models
{
	public class PaginatedList<T>
	{
		public PaginatedList()
		{
		}

		public PaginatedList(List<T> items, int totalItems, int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			TotalItems = totalItems;
			TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			Items.AddRange(items);
		}

		public bool HasNextPage => PageIndex < TotalPages;

		public bool HasPreviousPage => PageIndex > 1;

		public List<T> Items { get; set; } = new List<T>();

		public int PageIndex { get; set; }

		public int TotalItems { get; set; }

		public int TotalPages { get; set; }
	}
}