using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Categories")]
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CategoryID")]
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(15), MinLength(1)]
        public string CategoryName { get; set; }

        [Column("Description", TypeName = "ntext")]
        public string Description { get; set; }

        [Column("Picture", TypeName = "image")]
        public byte[] Picture { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
