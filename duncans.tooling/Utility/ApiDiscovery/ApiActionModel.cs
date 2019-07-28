// <copyright file="ApiActionModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.ApiDiscovery
{
    [NotMapped]
    public class ApiActionModel
    {
        public ApiActionModel()
        {
            this.Parameters = new List<ApiActionParameterModel>();
            this.SucessResponseCode = 200;
        }

        public ApiMethod Method { get; set; }

        [SqlInjectionCheck]
        public string Name { get; set; }

        [SqlInjectionCheck]
        public string Signature { get; set; }

        public List<ApiActionParameterModel> Parameters { get; set; }

        public Type ReturnType { get; set; }

        [SqlInjectionCheck]
        public string ReturnBody { get; set; }

        [SqlInjectionCheck]
        public string Route { get; set; }

        public int SucessResponseCode { get; set; }
    }
}
