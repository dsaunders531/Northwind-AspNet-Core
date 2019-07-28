// <copyright file="ViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using duncans.TagHelpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace duncans.ViewModel
{
    /// <summary>
    /// The base view model. To be used by every view model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    [NotMapped]
    public class ViewModel<TModel> : IRecordViewModel<TModel>
    {
        public ViewModel()
        {
            this.PageMeta = new PageMetaModel();
        }

        public ViewModel(TModel model)
        {
            this.ViewData = model;
            this.PageMeta = new PageMetaModel();
        }

        public IPageMeta PageMeta { get; set; }

        public TModel ViewData { get; set; }

        public string ReturnUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "The record(s) are applicable on this date.")]
        [WithinDateRange]
        public DateTime? DataDate { get; set; }
    }
}
