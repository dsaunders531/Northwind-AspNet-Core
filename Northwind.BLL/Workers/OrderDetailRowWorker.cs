using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class OrderDetailRowWorker : GenericWorker<OrderDetailDbModel, int, OrderDetailRowApiModel, string>
    {
        public OrderDetailRowWorker(IRepository<OrderDetailDbModel, int> repository) : base(repository)
        {
        }

        public override OrderDetailRowApiModel Create(OrderDetailRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.OrderId == apiRowModel.OrderId && model.ProductId == apiRowModel.ProductId);
        }

        public override List<OrderDetailRowApiModel> FetchAll()
        {
            return base.FetchAll(d => d.Product.ProductName);
        }

        public override OrderDetailRowApiModel Update(OrderDetailRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.OrderId == apiRowModel.OrderId && model.ProductId == apiRowModel.ProductId);
        }
    }
}
