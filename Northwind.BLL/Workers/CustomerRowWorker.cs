using tools.EF;
using tools.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class CustomerRowWorker : GenericWorker<Customer, string, CustomerRowApiO, string>
    {
        public CustomerRowWorker(IRepository<Customer, string> repository) : base(repository)
        {
        }

        public override CustomerRowApiO Create(CustomerRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName && model.ContactName == apiRowModel.ContactName);
        }

        public override List<CustomerRowApiO> FetchAll()
        {
            return base.FetchAll(c => c.CompanyName);
        }

        public override CustomerRowApiO Update(CustomerRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.CustomerId == apiRowModel.CustomerId);
        }
    }
}
