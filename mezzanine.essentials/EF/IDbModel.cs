using System.Collections.Generic;
using System.Text;

namespace mezzanine.EF
{
    /// <summary>
    /// The IDbModel interface.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDbModel<TKey>
    {
        TKey RowId { get; set; }

        byte[] ConcurrencyToken { get; set; }

        long? RowVersion { get; set; }

        bool? Deleted { get; set; }
    }
}
