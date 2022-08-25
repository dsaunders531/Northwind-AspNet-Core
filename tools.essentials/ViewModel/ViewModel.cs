using System.ComponentModel.DataAnnotations.Schema;
using tools.Models;

namespace tools.ViewModels
{
    /// <summary>
    /// The base view model. To be used by every view model.
    /// </summary>
    [NotMapped]
    public class ViewModel : IViewModel
    {
        public ViewModel()
        {
            PageMeta = new PageMetaModel();
            Pagination = null; // The default is null (no pagination)
        }

        public IPageMeta PageMeta { get; set; }

        public IPagination Pagination { get; set; }
    }
}
