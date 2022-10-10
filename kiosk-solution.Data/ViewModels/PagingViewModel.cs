using System;
using System.Linq;

namespace kiosk_solution.Data.ViewModels
{
    public class PagingViewModel<TResult>
    {
        public int Total { get; set; }
        public IQueryable<TResult> Data { get; set; }
    }
}