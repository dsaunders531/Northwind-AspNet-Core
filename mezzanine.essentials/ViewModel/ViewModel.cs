using mezzanine.TagHelpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace mezzanine.ViewModel
{
    /// <summary>
    /// The base view model. To be used by every view model.
    /// </summary>
    [NotMapped]
    public class ViewModel<TModel> : IRecordViewModel<TModel>
    {
        public ViewModel()
        {
            this.PageMeta = new PageMetaModel();
        }

        public IPageMeta PageMeta { get; set; }
        
        public TModel ViewData { get; set; }

        public string ReturnUrl { get; set; }
    }
}
