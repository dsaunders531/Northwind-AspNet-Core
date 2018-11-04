using mezzanine.TagHelpers;

namespace mezzanine.ViewModel
{
    /// <summary>
    /// The interface a view model.
    /// </summary>
    public interface IViewModel
    {        
        IPageMeta PageMeta { get; set; }

        IPagination Pagination { get; set; }
    }    
}
