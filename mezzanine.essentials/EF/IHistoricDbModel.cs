using System;

namespace mezzanine.EF
{
    /// <summary>
    /// The IHistoricDbModel interface.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IHistoricDbModel<TKey> : IDbModel<long>
    {
        TKey ModelId { get; set; }

        EFActionType Action { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
