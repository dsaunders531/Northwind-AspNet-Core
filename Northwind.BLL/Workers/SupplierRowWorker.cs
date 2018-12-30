using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class SupplierRowWorker : GenericWorker<SupplierDbModel, int, SupplierRowApiModel, string>
    {
        public SupplierRowWorker(IRepository<SupplierDbModel, int> repository) : base(repository)
        {
        }

        public override SupplierRowApiModel Create(SupplierRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName && model.PostalCode == apiRowModel.PostalCode);
        }

        public override List<SupplierRowApiModel> FetchAll()
        {
            return base.FetchAll(s => s.CompanyName);
        }

        public override SupplierRowApiModel Update(SupplierRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.RowId == apiRowModel.RowId);
        }
    }
}
