using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mezzanine.Models
{
    [NotMapped]
    public class ApiControllerModel
    {
        public ApiControllerModel()
        {
            Actions = new List<ApiActionModel>();
        }

        public string Name { get; set; }

        public string Route { get; set; }

        public List<ApiActionModel> Actions { get; set; }
    }
}
