using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace tools.Models
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
    }
}
