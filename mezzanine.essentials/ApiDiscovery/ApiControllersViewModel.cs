using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.ViewModel;
using mezzanine.TagHelpers;

namespace mezzanine.ApiDiscovery
{
    [NotMapped]
    public class ApiControllersViewModel : IViewModel
    {
        public ApiControllersViewModel() : base()
        {
            this.Controllers = new List<ApiControllerModel>();            
        }

        public List<ApiControllerModel> Controllers { get; set; }

        public IPageMeta PageMeta { get; set; }

        public IPagination Pagination { get; set; }

        public string ReturnUrl { get; set; }
    }
}
