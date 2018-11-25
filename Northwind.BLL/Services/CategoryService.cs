using System;
using System.Collections.Generic;
using System.Text;
using mezzanine.EF;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public sealed class CategoryService : CategoryWorker
    {
        public CategoryService(IRepository<CategoryDbModel, int> categories) : base(categories)
        {
        }
    }
}
