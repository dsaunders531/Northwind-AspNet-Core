// <copyright file="ListViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using duncans.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duncans.ViewModel
{
    public class ListViewModel<TModel> : IRecordsListViewModel<TModel>
    {
        public ListViewModel(List<TModel> models)
        {
            this.Configure();
            this.ViewList = models;
        }

        public ListViewModel(List<TModel> models, IPagination pagination)
        {
            this.Configure();
            this.ViewList = models;
            this.Pagination = pagination;
        }

        public ListViewModel()
        {
            this.Configure();
        }

        public IPagination Pagination { get; set; }

        public bool CanAdd { get; set; }

        public List<TModel> ViewList { get; set; }

        public IPageMeta PageMeta { get; set; }

        public string ReturnUrl { get; set; }

        public string SearchTerm { get; set; }

        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "The record(s) are applicable on this date.")]
        [WithinDateRange]
        public DateTime? DataDate { get; set; }

        private void Configure()
        {
            this.PageMeta = new PageMetaModel();
            this.Pagination = null;
            this.CanAdd = false;
            this.ViewList = new List<TModel>();
            this.SearchTerm = string.Empty;
            this.SortColumn = string.Empty;
            this.ReturnUrl = string.Empty;
        }
    }
}
