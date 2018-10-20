using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class SupplierRowWorker : GenericWorker<Supplier, int, SupplierRowApiO, string>
    {
        public SupplierRowWorker(IRepository<Supplier, int> repository) : base(repository)
        {
        }

        public override SupplierRowApiO Create(SupplierRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName && model.PostalCode == apiRowModel.PostalCode);
        }

        public override List<SupplierRowApiO> FetchAll()
        {
            return base.FetchAll(s => s.CompanyName);
        }

        public override SupplierRowApiO Update(SupplierRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.SupplierId == apiRowModel.SupplierId);
        }
    }
}
