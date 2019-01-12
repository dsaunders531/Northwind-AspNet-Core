using mezzanine.TagHelpers;
using System.Collections.Generic;

namespace mezzanine.ViewModel
{
    public class ListViewModel<TModel> : IRecordsListViewModel<TModel>
    {
        public ListViewModel()
        {
            this.PageMeta = new PageMetaModel();
            this.Pagination = null;
            this.CanAdd = false;
            this.ViewList = new List<TModel>();
        }

        public IPagination Pagination { get; set; }

        public bool CanAdd { get; set; }

        public List<TModel> ViewList { get; set; }

        public IPageMeta PageMeta { get; set; }

        public string ReturnUrl { get; set; }
    }
}
