// <copyright file="IPageMeta.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Utility;
using System;

namespace duncans.TagHelpers
{
    /// <summary>
    /// The IPageMeta interface.
    /// </summary>
    public interface IPageMeta
    {
        bool AddHtml5Defaults { get; set; }

        string PageTitle { get; set; }

        string Keywords { get; }

        string Description { get; set; }

        bool RobotsIndex { get; set; }

        bool RobotsFollow { get; set; }

        Uri ThumbnailIconPath { get; set; }

        AssemblyInfo AppInfo { get; set; }

        void AddKeyword(string value);
    }
}
