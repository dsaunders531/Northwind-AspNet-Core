using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class OrderRowWorker : GenericWorker<OrderDbModel, int, OrderRowApiModel, DateTime?>
    {
        public OrderRowWorker(IRepository<OrderDbModel, int> repository) : base(repository)
        {
        }

        public override OrderRowApiModel Create(OrderRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CustomerId == apiRowModel.CustomerId && model.EmployeeId == apiRowModel.EmployeeId);
        }

        public override List<OrderRowApiModel> FetchAll()
        {
            return base.FetchAll(o => o.OrderDate);
        }

        public override OrderRowApiModel Update(OrderRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.RowId == apiRowModel.RowId);
        }
    }
}
