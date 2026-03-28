using System.Collections.Generic;

namespace Yugen.Home.Budget.Data.Repositories
{
    public class BaseListDto<T>
    {
        public BaseListDto()
        {
        }

        public BaseListDto(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public List<T> Items { get; set; } = [];

        public int TotalCount { get; set; }
    }
}