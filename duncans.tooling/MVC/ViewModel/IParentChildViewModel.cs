// <copyright file="IParentChildViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.ViewModel
{
    public interface IParentChildViewModel<TParent, TChild> : IRecordViewModel<TParent>
    {
        IRecordsListViewModel<TChild> Children { get; set; }
    }
}
