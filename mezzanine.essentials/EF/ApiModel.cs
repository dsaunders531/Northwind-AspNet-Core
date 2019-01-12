using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace mezzanine.EF
{
    public abstract class ApiModel<TKey> : IApiModel<TKey>
    {
        [Required]
        public TKey RowId { get; set; }

        public bool Deleteable { get; set; }

        public bool Readonly { get; set; }
    }
}
