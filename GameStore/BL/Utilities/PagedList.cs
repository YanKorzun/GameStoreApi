using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BL.Utilities
{
    public class PagedList<T> : List<T>
    {
        public PagedList()
        {
        }

        public PagedList(IEnumerable<T> items, uint count, uint pageNumber, uint pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (uint)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public uint CurrentPage { get; }
        public uint TotalPages { get; }
        public uint PageSize { get; }
        public uint TotalCount { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public static PagedList<T> ToPagedList(IQueryable<T> source, uint pageNumber, uint pageSize)
        {
            var count = source.Count();
            var items = source.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize).ToList();
            return new(items, (uint)count, pageNumber, pageSize);
        }
    }
}