using tools.EF;
using tools.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class EmployeeTerritoryRowWorker : GenericWorker<EmployeeTerritory, int, EmployeeTerritoryRowApiO, string>
    {
        public EmployeeTerritoryRowWorker(IRepository<EmployeeTerritory, int> repository) : base(repository)
        {
        }

        public override EmployeeTerritoryRowApiO Create(EmployeeTerritoryRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, t => t.EmployeeId == apiRowModel.EmployeeId && t.TerritoryId == apiRowModel.TerritoryId);
        }

        public override List<EmployeeTerritoryRowApiO> FetchAll()
        {
            return base.FetchAll(t => t.Employee.LastName + t.Employee.FirstName);
        }

        public override EmployeeTerritoryRowApiO Update(EmployeeTerritoryRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, t => t.EmployeeId == apiRowModel.EmployeeId && t.TerritoryId == apiRowModel.TerritoryId);
        }
    }
}
