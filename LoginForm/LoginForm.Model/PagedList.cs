using System;
using System.Collections.Generic;

namespace LoginForm.Model
{
    public class PagedList<T> : List<T> where T : class
    {
        #region constructor
        public PagedList(List<T> list, int totalCount, int? pageSize, int? page) : base(list)
        {
            TotalCount = totalCount;
            if (pageSize.HasValue) PageSize = pageSize.Value;
            else PageSize = 25;

            if (page.HasValue) Page = page.Value;
            else Page = 1;
        }
        #endregion

        #region properties
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }
        #endregion
    }
}
