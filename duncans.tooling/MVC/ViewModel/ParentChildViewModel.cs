// <copyright file="ParentChildViewModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using duncans.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duncans.ViewModel
{
    public class ParentChildViewModel<TParent, TChild> : IParentChildViewModel<TParent, TChild>
    {
        public ParentChildViewModel()
        {
            this.Configure();
        }

        public ParentChildViewModel(TParent parent, List<TChild> children)
        {
            this.Configure();
            this.ViewData = parent;
            this.Children = new ListViewModel<TChild>(children);
        }

        public ParentChildViewModel(TParent parent, List<TChild> children, IPagination pagination)
        {
            this.Configure();
            this.ViewData = parent;
            this.Children = new ListViewModel<TChild>(children, pagination);
        }

        public IRecordsListViewModel<TChild> Children { get; set; }

        public TParent ViewData { get; set; }

        public IPageMeta PageMeta { get; set; }

        public string ReturnUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "The record(s) are applicable on this date.")]
        [WithinDateRange]
        public DateTime? DataDate { get; set; }

        private void Configure()
        {
            this.PageMeta = new PageMetaModel();
            this.Children = new ListViewModel<TChild>();
        }
    }
}
