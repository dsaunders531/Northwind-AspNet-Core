// <copyright file="DoNotDiscoverAttribute.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans.ApiDiscovery
{
    /// <summary>
    /// Attribute for api endpoints and controllers which must not be shown in the api discovery page.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class DoNotDiscoverAttribute : Attribute { }
}
