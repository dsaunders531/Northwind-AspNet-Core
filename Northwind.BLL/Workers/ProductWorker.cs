using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class ProductWorker : GenericWorker<Product, int, ProductRowApiO, string>
    {
        public ProductWorker(IRepository<Product, int> repository) : base(repository)
        {
        }

        public override List<ProductRowApiO> FetchAll()
        {
            return base.FetchAll(p => p.ProductName);
        }

        public override ProductRowApiO Create(ProductRowApiO apiModel)
        {
            return base.Create(apiModel, model => model.ProductId == apiModel.ProductId);
        }

        public override ProductRowApiO Update(ProductRowApiO apiModel)
        {
            return base.Update(apiModel, model => model.ProductId == apiModel.ProductId);
        }
    }
}
