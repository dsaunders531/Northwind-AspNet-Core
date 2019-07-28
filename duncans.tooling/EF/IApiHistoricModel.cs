// <copyright file="IApiHistoricModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.EF
{
    public interface IApiHistoricModel<TKey> : IApiModel<long>
    {
        TKey ModelId { get; set; }

        EFActionType Action { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
