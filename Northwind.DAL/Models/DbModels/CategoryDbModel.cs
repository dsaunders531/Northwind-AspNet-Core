using mezzanine.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Categories")]
    public partial class CategoryDbModel
    {
        public CategoryDbModel()
        {
            Products = new HashSet<ProductDbModel>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CategoryID")]
        [Required]
        public int CategoryId { get; set; }

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
