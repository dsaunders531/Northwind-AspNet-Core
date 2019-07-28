// <copyright file="IRecordsListViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.TagHelpers;
using System.Collections.Generic;

namespace duncans.ViewModel
{
    public interface IRecordsListViewModel : IViewModel
    {
        IPagination Pagination { get; set; }

        bool CanAdd { get; set; }

        string SearchTerm { get; set; }

        string SortColumn { get; set; }

        SortDirection SortDirection { get; set; }
    }

    public interface IRecordsListViewModel<T> : IRecordsListViewModel
    {
        List<T> ViewList { get; set; }
    }
}
