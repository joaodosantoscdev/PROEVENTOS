using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList(List<T> items, int count, int pgNumber, int pgSize)
        {
            TotalCount = count;
            PageSize = pgSize;
            CurrentPage = pgNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pgSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync( IQueryable<T> source, 
                                                           int pgNumber, 
                                                           int pgSize )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();

            return new PageList<T>(items, count, pgNumber, pgSize);
        }
    }
}
