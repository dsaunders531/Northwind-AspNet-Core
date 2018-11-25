using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class ShipperRowWorker : GenericWorker<ShipperDbModel, int, ShipperRowApiModel, string>
    {
        public ShipperRowWorker(IRepository<ShipperDbModel, int> repository) : base(repository)
        {
        }

        public override ShipperRowApiModel Create(ShipperRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName);
        }

        public override List<ShipperRowApiModel> FetchAll()
        {
            return base.FetchAll(s => s.CompanyName);
        }

        public override ShipperRowApiModel Update(ShipperRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.ShipperId == apiRowModel.ShipperId);
        }
    }
}
