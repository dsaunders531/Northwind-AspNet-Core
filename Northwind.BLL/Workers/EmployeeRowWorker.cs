using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class EmployeeRowWorker : GenericWorker<EmployeeDbModel, int, EmployeeRowApiModel, string>
    {
        public EmployeeRowWorker(IRepository<EmployeeDbModel, int> repository) : base(repository)
        {
        }

        public override EmployeeRowApiModel Create(EmployeeRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, e => e.FirstName == apiRowModel.FirstName && e.LastName == apiRowModel.LastName);
        }

        public override List<EmployeeRowApiModel> FetchAll()
        {
            return base.FetchAll(e => e.FirstName + e.FirstName );
        }

        public override EmployeeRowApiModel Update(EmployeeRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.EmployeeId == apiRowModel.EmployeeId);
        }
    }
}
