﻿using tools.EF;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public sealed class CategoryService : CategoryWorker
    {
        public CategoryService(IRepository<Category, int> categories) : base(categories)
        {
        }
    }
}
