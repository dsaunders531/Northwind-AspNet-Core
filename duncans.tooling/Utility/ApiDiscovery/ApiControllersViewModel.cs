// <copyright file="ApiControllersViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using duncans.TagHelpers;
using duncans.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.ApiDiscovery
{
    [NotMapped]
    public class ApiControllersViewModel : IViewModel
    {
        public ApiControllersViewModel() : base()
        {
            this.Controllers = new List<ApiControllerModel>();
        }

        public List<ApiControllerModel> Controllers { get; set; }

        public IPageMeta PageMeta { get; set; }

        public IPagination Pagination { get; set; }

        public string ReturnUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "The record(s) are applicable on this date.")]
        [WithinDateRange]
        public DateTime? DataDate { get; set; }
    }
}
