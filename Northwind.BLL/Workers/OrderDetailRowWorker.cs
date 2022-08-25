using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;
using tools.EF;
using tools.WorkerPattern;

namespace Northwind.BLL.Workers
{
    public class OrderDetailRowWorker : GenericWorker<OrderDetail, int, OrderDetailRowApiO, string>
    {
        public OrderDetailRowWorker(IRepository<OrderDetail, int> repository) : base(repository)
        {
        }

        public override OrderDetailRowApiO Create(OrderDetailRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.OrderId == apiRowModel.OrderId && model.ProductId == apiRowModel.ProductId);
        }

        public override List<OrderDetailRowApiO> FetchAll()
        {
            return base.FetchAll(d => d.Product.ProductName);
        }

        public override OrderDetailRowApiO Update(OrderDetailRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.OrderId == apiRowModel.OrderId && model.ProductId == apiRowModel.ProductId);
        }
    }
}
