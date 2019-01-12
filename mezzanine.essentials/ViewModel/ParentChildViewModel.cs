using mezzanine.TagHelpers;

namespace mezzanine.ViewModel
{
    public class ParentChildViewModel<TParent, TChild> : IParentChildViewModel<TParent, TChild>
    {
        public ParentChildViewModel()
        {
            this.PageMeta = new PageMetaModel();
            this.Children = new ListViewModel<TChild>();
        }

        public IRecordsListViewModel<TChild> Children { get; set; }

        public TParent ViewData { get; set; }

        public IPageMeta PageMeta { get; set; }

        public string ReturnUrl { get; set; }
    }
}
