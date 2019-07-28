// <copyright file="ApiControllerModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.ApiDiscovery
{
    [NotMapped]
    public class ApiControllerModel
    {
        public ApiControllerModel()
        {
            this.Actions = new List<ApiActionModel>();
        }

        [SqlInjectionCheck]
        public string Name { get; set; }

        [SqlInjectionCheck]
        public string Route { get; set; }

        public List<ApiActionModel> Actions { get; set; }
    }
}
