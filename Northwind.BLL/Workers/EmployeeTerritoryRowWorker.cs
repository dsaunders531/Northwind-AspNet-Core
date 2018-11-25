using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class EmployeeTerritoryRowWorker : GenericWorker<EmployeeTerritoryDbModel, int, EmployeeTerritoryRowApiModel, string>
    {
        public EmployeeTerritoryRowWorker(IRepository<EmployeeTerritoryDbModel, int> repository) : base(repository)
        {
        }

        public override EmployeeTerritoryRowApiModel Create(EmployeeTerritoryRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, t => t.EmployeeId == apiRowModel.EmployeeId && t.TerritoryId == apiRowModel.TerritoryId);
        }

        public override List<EmployeeTerritoryRowApiModel> FetchAll()
        {
            return base.FetchAll(t => t.Employee.LastName + t.Employee.FirstName);
        }

        public override EmployeeTerritoryRowApiModel Update(EmployeeTerritoryRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, t => t.EmployeeId == apiRowModel.EmployeeId && t.TerritoryId == apiRowModel.TerritoryId);
        }
    }
}
