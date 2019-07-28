// <copyright file="IViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.TagHelpers;
using System;

namespace duncans.ViewModel
{
    /// <summary>
    /// The interface a view model.
    /// </summary>
    public interface IViewModel
    {
        IPageMeta PageMeta { get; set; }

        string ReturnUrl { get; set; }

        DateTime? DataDate { get; set; }
    }
}
