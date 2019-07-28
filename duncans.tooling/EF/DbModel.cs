// <copyright file="DbModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.EF
{
    /// <summary>
    /// The base class for Models.
    /// </summary>
    /// <typeparam name="TKey">The type for the RowId column.</typeparam>
    public class DbModel<TKey> : IDbModel<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey RowId { get; set; }

        [Timestamp]
        public byte[] ConcurrencyToken { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long? RowVersion { get; set; } = 0;

        public bool Deleted { get; set; } = false;
    }
}
