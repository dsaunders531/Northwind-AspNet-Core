﻿using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class ShipperRowWorker : GenericWorker<Shipper, int, ShipperRowApiO, string>
    {
        public ShipperRowWorker(IRepository<Shipper, int> repository) : base(repository)
        {
        }

        public override ShipperRowApiO Create(ShipperRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.CompanyName == apiRowModel.CompanyName);
        }

        public override List<ShipperRowApiO> FetchAll()
        {
            return base.FetchAll(s => s.CompanyName);
        }

        public override ShipperRowApiO Update(ShipperRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.ShipperId == apiRowModel.ShipperId);
        }
    }
}
