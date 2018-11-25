using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class RegionRowWorker : GenericWorker<RegionDbModel, int, RegionRowApiModel, string>
    {
        public RegionRowWorker(IRepository<RegionDbModel, int> repository) : base(repository)
        {
        }

        public override RegionRowApiModel Create(RegionRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.RegionDescription == apiRowModel.RegionDescription);
        }

        public override List<RegionRowApiModel> FetchAll()
        {
            return base.FetchAll(r => r.RegionDescription);
        }

        public override RegionRowApiModel Update(RegionRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.RegionId == apiRowModel.RegionId);
        }
    }
}
