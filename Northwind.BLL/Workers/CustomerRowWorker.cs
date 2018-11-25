using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class CustomerRowWorker : GenericWorker<CustomerDbModel, string, CustomerRowApiModel, string>
    {
        public CustomerRowWorker(IRepository<CustomerDbModel, string> repository) : base(repository)
        {
        }

        public override CustomerRowApiModel Create(CustomerRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName && model.ContactName == apiRowModel.ContactName);
        }

        public override List<CustomerRowApiModel> FetchAll()
        {
            return base.FetchAll(c => c.CompanyName);
        }

        public override CustomerRowApiModel Update(CustomerRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.CustomerId == apiRowModel.CustomerId);
        }
    }
}
