using mezzanine.EF;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class TerritoryRowWorker : GenericWorker<TerritoryDbModel, int, TerritoryRowApiModel, string>
    {
        public TerritoryRowWorker(IRepository<TerritoryDbModel, int> repository) : base(repository)
        {
        }

        public override TerritoryRowApiModel Create(TerritoryRowApiModel apiRowModel)
        {
            return base.Create(apiRowModel, model => model.RegionId == apiRowModel.RegionId 
                                                    && model.TerritoryDescription == apiRowModel.TerritoryDescription);
        }

        public override List<TerritoryRowApiModel> FetchAll()
        {
            return base.FetchAll(t => t.TerritoryDescription);
        }

        public override TerritoryRowApiModel Update(TerritoryRowApiModel apiRowModel)
        {
            return base.Update(apiRowModel, model => model.TerritoryId == apiRowModel.TerritoryId);
        }
    }
}
