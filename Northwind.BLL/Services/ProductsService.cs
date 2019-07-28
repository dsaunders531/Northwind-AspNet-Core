using System.Collections.Generic;
using duncans.EF;
using duncans.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;

namespace Northwind.BLL.Services
{
    public class ProductsService : GenericService<ProductDbModel, int, ProductApiModel, string>, IGenericService<ProductDbModel, ProductApiModel, int>
    {
        public ProductsService(IRepository<ProductDbModel, int> repository) : base(repository)
        {
        }

        public override ProductApiModel Create(ProductApiModel apiRowModel)
        {
            return base.Create(apiRowModel, new System.Func<ProductDbModel, bool>(p => p.ProductName == apiRowModel.ProductName && p.SupplierId == apiRowModel.SupplierId && p.CategoryId == apiRowModel.CategoryId));
        }

        public override List<ProductApiModel> FetchAll()
        {
            return base.FetchAll(new System.Func<ProductDbModel, string>(p => p.ProductName));
        }

        public override ProductApiModel Update(ProductApiModel apiRowModel)
        {
            return base.Update(apiRowModel, new System.Func<ProductDbModel, bool>(p => p.ProductName == apiRowModel.ProductName && p.SupplierId == apiRowModel.SupplierId && p.CategoryId == apiRowModel.CategoryId));
        }
    }
}
