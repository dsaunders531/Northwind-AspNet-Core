using System.Collections.Generic;
using duncans.EF;
using duncans.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public class CategoriesService : GenericService<CategoryDbModel, int, CategoryApiModel, string>, IGenericService<CategoryDbModel, CategoryApiModel, int>
    {
        public CategoriesService(IRepository<CategoryDbModel, int> repository) : base(repository)
        {
        }

        public override CategoryApiModel Create(CategoryApiModel apiRowModel)
        {
            return base.Create(apiRowModel, new System.Func<CategoryDbModel, bool>(c => c.CategoryName == apiRowModel.CategoryName));
        }

        public override List<CategoryApiModel> FetchAll()
        {
            return base.FetchAll(new System.Func<CategoryDbModel, string>(c => c.CategoryName));
        }

        public override CategoryApiModel Update(CategoryApiModel apiRowModel)
        {
            return base.Update(apiRowModel, new System.Func<CategoryDbModel, bool>(c => c.CategoryName == apiRowModel.CategoryName));
        }
    }
}
