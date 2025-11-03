using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
           this IQueryable<T> query,
           int page,
           int pageSize,
           CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalCount = await query.CountAsync(ct);

            // Om page är 2 och pageSize är 10
            // Då skippas 2 - 1 * pageSize (10) och tar sedan 10 rekords. då får man andra sidan

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }

    public class ListRequest
    {
        [FromQuery] public int Page { get; set; } = 1;
        [FromQuery] public int PageSize { get; set; } = 10;
    }


    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

}
