// <copyright file="PaginatedResult.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.TagHelpers;
using System.Collections.Generic;

namespace duncans.WorkerPattern
{
    /// <summary>
    /// Model for storing a list of things and pagination data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedResult<T>
    {
        public PaginatedResult()
        {
            this.Pagination = new PaginationModel();
            this.List = new List<T>();
        }

        public IPagination Pagination { get; set; }

        public List<T> List { get; set; }
    }
}
