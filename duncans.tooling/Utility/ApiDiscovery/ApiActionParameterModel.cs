// <copyright file="ApiActionParameterModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.ApiDiscovery
{
    /// <summary>
    /// Query parameters for an api action.
    /// </summary>
    [NotMapped]
    public class ApiActionParameterModel
    {
        [SqlInjectionCheck]
        public string Name { get; set; }

        [SqlInjectionCheck]
        public string DefaultValue { get; set; }

        public Type Type { get; set; }

        public bool IsNullable { get; set; }

        public ApiActionParameterType ParameterType { get; set; }
    }
}
