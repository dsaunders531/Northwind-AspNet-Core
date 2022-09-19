using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace tools.Models
{
    [NotMapped]
    public class ApiActionModel
    {
        public ApiActionModel()
        {
            this.Parameters = new List<ApiActionParameterModel>();
            this.SucessResponseCode = 200;
        }

        public ApiMethod Method { get; set; }

        public string Name { get; set; }

        public string Signature { get; set; }

        public List<ApiActionParameterModel> Parameters { get; set; }

        public Type ReturnType { get; set; }

        public string ReturnBody { get; set; }

        public string Route { get; set; }

        public int SucessResponseCode { get; set; }
    }
}
