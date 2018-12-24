using System;
using System.ComponentModel.DataAnnotations;

namespace mezzanine.EF
{
    /// <summary>
    /// The base class for historic models
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class HistoricDbModel<TKey> : DbModel<long>, IHistoricDbModel<TKey>
    {
        public HistoricDbModel()
        {
            this.StartDate = DateTime.Now;
        }

        public TKey ModelId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(EFActionType))]
        public EFActionType Action { get; set; }
    }
}
