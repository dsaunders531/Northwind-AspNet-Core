using mezzanine.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace mezzanine.ViewModels
{
    /// <summary>
    /// The base view model. To be used by every view model.
    /// </summary>
    [NotMapped]
    public class ViewModel : IViewModel
    {
        public ViewModel()
        {
            this.PageMeta = new PageMetaModel();
            this.Pagination = null; // The default is null (no pagination)
        }

        public IPageMeta PageMeta { get; set; }

        public IPagination Pagination { get; set; }
    }
}
