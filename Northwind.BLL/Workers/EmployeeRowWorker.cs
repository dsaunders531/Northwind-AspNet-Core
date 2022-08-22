using tools.EF;
using tools.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class EmployeeRowWorker : GenericWorker<Employee, int, EmployeeRowApiO, string>
    {
        public EmployeeRowWorker(IRepository<Employee, int> repository) : base(repository)
        {
        }

        public override EmployeeRowApiO Create(EmployeeRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, e => e.FirstName == apiRowModel.FirstName && e.LastName == apiRowModel.LastName);
        }

        public override List<EmployeeRowApiO> FetchAll()
        {
            return base.FetchAll(e => e.FirstName + e.FirstName);
        }

        public override EmployeeRowApiO Update(EmployeeRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.EmployeeId == apiRowModel.EmployeeId);
        }
    }
}
