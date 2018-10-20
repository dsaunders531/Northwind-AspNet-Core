using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class RegionRowWorker : GenericWorker<Region, int, RegionRowApiO, string>
    {
        public RegionRowWorker(IRepository<Region, int> repository) : base(repository)
        {
        }

        public override RegionRowApiO Create(RegionRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.RegionDescription == apiRowModel.RegionDescription);
        }

        public override List<RegionRowApiO> FetchAll()
        {
            return base.FetchAll(r => r.RegionDescription);
        }

        public override RegionRowApiO Update(RegionRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.RegionId == apiRowModel.RegionId);
        }
    }
}
