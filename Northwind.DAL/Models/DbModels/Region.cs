using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        [Key]
        [Column("RegionID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegionDescription { get; set; }

        public ICollection<Territory> Territories { get; set; }
    }
}
