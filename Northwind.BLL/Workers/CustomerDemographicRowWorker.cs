using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class CustomerDemographicRowWorker : GenericWorker<CustomerDemographic, string, CustomerDemographicRowApiO, string>
    {
        public CustomerDemographicRowWorker(IRepository<CustomerDemographic, string> repository) : base(repository)
        {
        }

        public override CustomerDemographicRowApiO Create(CustomerDemographicRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CustomerDesc == apiRowModel.CustomerDesc);
        }

        public override List<CustomerDemographicRowApiO> FetchAll()
        {
            return base.FetchAll(c => c.CustomerDesc);
        }

        public override CustomerDemographicRowApiO Update(CustomerDemographicRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.CustomerTypeId == apiRowModel.CustomerTypeId);
        }
    }
}
