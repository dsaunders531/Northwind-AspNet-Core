// <copyright file="IRecordViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.ViewModel
{
    public interface IRecordViewModel<T> : IViewModel
    {
        T ViewData { get; set; }
    }
}
