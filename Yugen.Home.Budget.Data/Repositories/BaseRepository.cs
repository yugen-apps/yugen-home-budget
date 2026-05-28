using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Yugen.Home.Budget.Data.Repositories
{
	public class BaseRepository
	{
		protected async Task<BaseListDto<T>> GetListAsync<T>(IQueryable<T> query, int page, int pageSize)
		{
			//query = FilterResults(query, filter);

			var totalCount = query.Count();

			query = PaginateResults(query, page, pageSize);

			var pagedResults = await query.ToListAsync();

			return new BaseListDto<T>(pagedResults, totalCount);
		}

		//protected IQueryable<T> FilterResults<T>(IQueryable<T> query, string filter)
		//{
		//    if (string.IsNullOrWhiteSpace(filter))
		//    {
		//        return query;
		//    }

		//    query = query.Where(x =>
		//        x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));

		//    return query;
		//}

		protected IQueryable<T> PaginateResults<T>(IQueryable<T> query, int page, int pageSize)
		{
			if (page > 0 &&
				pageSize > 0)
			{
				query = query.Skip((page - 1) * pageSize);
			}

			if (pageSize > 0)
			{
				query = query.Take(pageSize);
			}

			return query;
		}
	}
}