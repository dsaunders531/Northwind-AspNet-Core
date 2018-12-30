using mezzanine.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    [Table("Categories")]
    public partial class CategoryDbModel : DbModel<int>
    {
        public CategoryDbModel()
        {
            Products = new HashSet<ProductDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Required]
        [MaxLength(15), MinLength(1)]
        [SqlInjectionCheck]
        public string CategoryName { get; set; }

        [Column("Description", TypeName = "ntext")]
        [SqlInjectionCheck]
        public string Description { get; set; }

        [Column("Picture", TypeName = "image")]
        public byte[] Picture { get; set; }

        public ICollection<ProductDbModel> Products { get; set; }
    }
}
