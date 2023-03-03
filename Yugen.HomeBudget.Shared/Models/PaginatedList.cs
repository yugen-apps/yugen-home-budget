namespace Yugen.HomeBudget.Shared.Models
{
    public class PaginatedList<T>
    {
        public PaginatedList()
        {

        }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.Items = new List<T>();
            this.Items.AddRange(items);
        }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
    }
}
