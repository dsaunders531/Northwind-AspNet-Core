using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;
using tools.EF;
using tools.WorkerPattern;

namespace Northwind.BLL.Workers
{
    public class ProductRowWorker : GenericWorker<Product, int, ProductRowApiO, string>
    {
        public ProductRowWorker(IRepository<Product, int> repository) : base(repository)
        {
        }

        public override List<ProductRowApiO> FetchAll()
        {
            return base.FetchAll(p => p.ProductName);
        }

        public override ProductRowApiO Create(ProductRowApiO apiModel)
        {
            return base.Create(apiModel, model => model.ProductName == apiModel.ProductName
                                        && model.CategoryId == apiModel.CategoryId
                                        && model.SupplierId == apiModel.SupplierId);
        }

        public override ProductRowApiO Update(ProductRowApiO apiModel)
        {
            return base.Update(apiModel, model => model.ProductId == apiModel.ProductId);
        }
    }
}
