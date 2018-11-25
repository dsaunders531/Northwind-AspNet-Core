using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class ProductRowWorker : GenericWorker<ProductDbModel, int, ProductRowApiModel, string>
    {
        public ProductRowWorker(IRepository<ProductDbModel, int> repository) : base(repository)
        {
        }

        public override List<ProductRowApiModel> FetchAll()
        {
            return base.FetchAll(p => p.ProductName);
        }

        public override ProductRowApiModel Create(ProductRowApiModel apiModel)
        {
            return base.Create(apiModel, model => model.ProductName == apiModel.ProductName 
                                        && model.CategoryId == apiModel.CategoryId 
                                        && model.SupplierId == apiModel.SupplierId);
        }

        public override ProductRowApiModel Update(ProductRowApiModel apiModel)
        {
            return base.Update(apiModel, model => model.ProductId == apiModel.ProductId);
        }
    }
}
