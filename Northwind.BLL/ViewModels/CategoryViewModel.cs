﻿using Northwind.BLL.Models;
using System.Collections.Generic;
using tools.ViewModels;

namespace Northwind.BLL.ViewModels
{
    public class CategoryViewModel : ViewModel
    {
        public CategoryRowApiO Category { get; set; }

        public List<ProductApiO> Products { get; set; }
    }
}
