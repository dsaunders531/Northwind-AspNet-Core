// <copyright file="HistoricDbModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.EF
{
    /// <summary>
    /// The base class for historic models.
    /// </summary>
    /// <typeparam name="TKey">The type of key used by the mode we want to keep history records for.</typeparam>
    public class HistoricDbModel<TKey> : DbModel<long>, IHistoricDbModel<TKey>
    {
        public HistoricDbModel()
        {
            this.StartDate = DateTime.UtcNow;
        }

        public TKey ModelId { get; set; }

        [WithinDateRange]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [WithinDateRange]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(EFActionType))]
        public EFActionType Action { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public new long? RowVersion { get; set; } = 0;
    }
}
