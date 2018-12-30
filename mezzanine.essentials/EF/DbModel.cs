using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace mezzanine.EF
{
    /// <summary>
    /// The base class for Models
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DbModel<TKey> : IDbModel<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey RowId { get; set; }

        [Timestamp]
        public byte[] ConcurrencyToken { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long? RowVersion { get; set; } = 0;

        public bool? Deleted { get; set; } = false;
    }
}
