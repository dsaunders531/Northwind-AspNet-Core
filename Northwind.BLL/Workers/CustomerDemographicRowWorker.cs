using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class CustomerDemographicRowWorker : GenericWorker<CustomerDemographicDbModel, string, CustomerDemographicRowApiModel, string>
    {
        public CustomerDemographicRowWorker(IRepository<CustomerDemographicDbModel, string> repository) : base(repository)
        {
        }

        public override CustomerDemographicRowApiModel Create(CustomerDemographicRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CustomerDesc == apiRowModel.CustomerDesc);
        }

        public override List<CustomerDemographicRowApiModel> FetchAll()
        {
            return base.FetchAll(c => c.CustomerDesc);
        }

        public override CustomerDemographicRowApiModel Update(CustomerDemographicRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.CustomerTypeId == apiRowModel.CustomerTypeId);
        }
    }
}
