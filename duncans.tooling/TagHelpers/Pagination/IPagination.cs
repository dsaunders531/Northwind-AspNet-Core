// <copyright file="IPagination.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans.TagHelpers
{
    /// <summary>
    /// The pagination interface
    /// </summary>
    public interface IPagination
    {
        int ItemCount { get; set; }

        int ItemsPerPage { get; set; }

        int CurrentPage { get; set; }

        string PageAction { get; set; }

        int PageCount();
    }
}
