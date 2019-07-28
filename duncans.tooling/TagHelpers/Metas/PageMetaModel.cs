// <copyright file="PageMetaModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace duncans.TagHelpers
{
    /// <summary>
    /// The page meta model for defining the page meta tags using the meta tag helper.
    /// </summary>
    public sealed class PageMetaModel : PageMeta
    {
        public PageMetaModel()
        {
            // Set the defaults
            this.AddHtml5Defaults = true;
            this.AppInfo = new duncans.Utility.AssemblyInfo();
            this.RobotsFollow = true;
            this.RobotsIndex = true;
        }
    }
}
