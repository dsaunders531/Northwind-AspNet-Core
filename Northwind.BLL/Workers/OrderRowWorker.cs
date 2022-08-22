using tools.EF;
using tools.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class OrderRowWorker : GenericWorker<Order, int, OrderRowApiO, DateTime?>
    {
        public OrderRowWorker(IRepository<Order, int> repository) : base(repository)
        {
        }

        public override OrderRowApiO Create(OrderRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CustomerId == apiRowModel.CustomerId && model.EmployeeId == apiRowModel.EmployeeId);
        }

        public override List<OrderRowApiO> FetchAll()
        {
            return base.FetchAll(o => o.OrderDate);
        }

        public override OrderRowApiO Update(OrderRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.OrderId == apiRowModel.OrderId);
        }
    }
}
