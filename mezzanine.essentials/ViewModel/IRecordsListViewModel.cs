using mezzanine.TagHelpers;
using System.Collections.Generic;

namespace mezzanine.ViewModel
{
    public interface IRecordsListViewModel<T> : IViewModel
    {
        IPagination Pagination { get; set; }

        bool CanAdd { get; set; }

        List<T> ViewList { get; set; }
    }
}
