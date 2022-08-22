using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    /// <summary>
    /// The public api version of Category
    /// </summary>
    [NotMapped]
    public class CategoryRowApiO
    {
        [Required]
        public int CategoryId { get; set; }

        [MaxLength(15), MinLength(1)]
        public string CategoryName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
